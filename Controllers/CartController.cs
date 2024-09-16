using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using Shop_ex.Models;
using Shop_ex.Repositories.CartRepository;
using Shop_ex.Services;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Shop_ex.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int userId;
        public CartController(ICartRepository cartRepository, IHttpContextAccessor httpContextAccessor)
        {
            _cartRepository = cartRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
		{
			var userRole = Convert.ToInt32(User.FindFirstValue(ClaimTypes.Role));
			ViewBag.UserRole = userRole;
			var session = _httpContextAccessor.HttpContext.Session;
			int? cartId = session.GetInt32("CartId");
			userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

			RegistrationModel registeredModel;
			if (userRole == 2)
			{
				var user = await _cartRepository.GetUserAsync(userId);
				registeredModel = new RegistrationModel { Username = user.Username, Email = user.Email, Telephone = user.Telephone };
			}
			else
			{
				registeredModel = new RegistrationModel();
			}

            var cartOrderModel = new CartOrderModel();
            if (cartId.HasValue)
            {
                var cart = await _cartRepository.GetCartAsync(cartId.Value);
                var order = new Order();
                cartOrderModel = new CartOrderModel
                {
                    Cart = cart,
                    Order = order,
                    RegistrationUser = registeredModel
                };
            }

            return View(cartOrderModel);
        }

		public IActionResult Order()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ValidateData(Order order, int totalPrice)
        {           
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Any())
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return Json(new { success = false, errors });
            }
            var error = new Dictionary<string, string>();
            await OrderAsync(order.UserName, order.UserPhone, order.UserEmail, totalPrice, error);

            if (error.Count > 0)
            {
                return BadRequest(error);
            }
            return Json(new { success = true, cartItemCount = 0 });
        }

        
        public async Task<IActionResult> OrderAsync(
                                                    string userName,
                                                    string userPhone,
                                                    string userEmail,
                                                    int totalPrice,
                                                    Dictionary<string, string> errors)
        {
            var cartItemCount = 0;

            using (var transaction = await _cartRepository.BeginTransactionAsync())
            {
                try
                {
                    Cart cart = await GetCartAsync();
                    cart.TotalPrice = totalPrice;
                    Order order;

                    int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (userId == 0)
                    {
                        order = new Order { OrderDate = DateTime.Now, CartId = cart.Id, UserName = userName, UserPhone = userPhone, UserEmail = userEmail };
                    }
                    else
                    {
                        order = new Order { OrderDate = DateTime.Now, CartId = cart.Id, UserName = userName, UserPhone = userPhone, UserEmail = userEmail, UserId = userId };
                    }

                    // Обновление количества товаров
                    foreach (var item in cart.Items)
                    {
                        var autoPart = await _cartRepository.GetAutoPartAsync(item.AutoPartId);
                        if (autoPart != null)
                        {
                            if (autoPart.Count < item.Quantity)
                            {
                                errors.Add("Quantity", $"Недостаточно товара {autoPart.Name} на складе.");
                                //return BadRequest(new { errors = errors.Values });
                                return BadRequest(new { errors });
                            }
                            autoPart.Count -= item.Quantity;
                            await _cartRepository.UpdateAutoPartAsync(autoPart);
                        }
                    }

                    await _cartRepository.UpdateCartAsync(cart);
                    await _cartRepository.CreateOrderAsync(order);
                    string itemsInCart = string.Join("\n", cart.Items.Select(i => $"{i.AutoPart.Name} - {i.Quantity}"));
                    int lastInsertedId = await _cartRepository.GetOrderCountAsync();
                    EmailService email = new EmailService();
                    var emailList = new List<string>
                    {
                        order.UserEmail,
                        "shtutgart22@mail.ru"
                    };

                    email.SendMail(emailList,
                                   $"Заказ номер {lastInsertedId + 1}",
                                   $"Имя клиента: {order.UserName}\n" +
                                   $"Телефон: {order.UserPhone}\n" +
                                   $"Дата заказа: {order.OrderDate}\n" +
                                   $"Детали заказа:\n{itemsInCart}\n" +
                                   $"Общая стоимость заказа: {totalPrice} руб.");
                    await _cartRepository.SaveChangesAsync();

                    // Подтверждаем транзакцию
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    // Откат транзакции в случае ошибки
                    await transaction.RollbackAsync();
                    errors.Add("Email", "Неверный email пользователя!");

                    return BadRequest(new { errors = errors.Values });
                }
            }

            ISession session = _httpContextAccessor.HttpContext.Session;
            session.SetInt32("CartId", 0);
            return Json(new { cartItemCount = cartItemCount });
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int autoPartId, int count)
        {
            var autoPart = await _cartRepository.GetAutoPartAsync(autoPartId);
            if (autoPart != null)
            {
                var cart = await GetCartAsync();
                var existingCartItem = cart.Items.FirstOrDefault(item => item.AutoPartId == autoPartId); //проверяем наличие товара в корзине

                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += count;

                    if (existingCartItem.Quantity > autoPart.Count)
                    {
                        existingCartItem.Quantity = (int)autoPart.Count;
                        //await _cartRepository.UpdateCartItemAsync(cart.Id, cart.Items.Count);
                        await _cartRepository.UpdateCartItemAsync(cart.Id, existingCartItem.Quantity);
                        return StatusCode(500, cart.Items.Count);   // Возвращаем количество элементов в корзине с кодом 500
                    }

                    await _cartRepository.UpdateCartItemAsync(cart.Id, cart.Items.Count);
                    return Ok(cart.Items.Count);// Возвращаем количество элементов в корзине с кодом 200 (Ok)
                }

                var cartItem = new CartItem { AutoPart = autoPart, Quantity = count };
                await _cartRepository.AddCartItemAsync(cart, cartItem);
                return Ok(cart.Items.Count); // Возвращаем количество элементов в корзине с кодом 200 (Ok)               
            }
            return NotFound();
        }   

        private async Task<Cart> GetCartAsync()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            int? cartId = session.GetInt32("CartId");

            if (!cartId.HasValue || cartId==0)
            {
                var cart = await _cartRepository.CreateCartAsync();
                session.SetInt32("CartId", cart.Id);
                return cart;
            }
            return await _cartRepository.GetCartAsync(cartId.Value);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCartItem(int cartItemId)
        {
            var cartItemCount = await _cartRepository.RemoveCartItemAsync(cartItemId);
            if (cartItemCount >= 0)
            {
                return Json(new { cartItemCount });
            }
            return BadRequest();
        }

        // Обновление количества товара в корзине
        [HttpPost]
        public async Task<IActionResult> UpdateCount(int cartItemId, int quantity)
        {
            try
            {
                await _cartRepository.UpdateCartItemAsync(cartItemId, quantity);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}