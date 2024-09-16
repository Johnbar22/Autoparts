using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop_ex.Models;
using Shop_ex.Repositories.OrderRepository;
using System.Security.Claims;

namespace Shop_ex.Controllers
{
	public class OrderController : Controller
	{
        private readonly IOrderRepository _orderRepository;
		public OrderController(IOrderRepository orderRepository)
		{
			_orderRepository = orderRepository;
		}
        public IActionResult Index()
		{
            var userRole = Convert.ToInt32(User.FindFirstValue(ClaimTypes.Role));
            ViewBag.UserRole = userRole;
            return View();
		}

		[HttpGet]
		public async Task<IActionResult> GetOrders(DateTime? date)
		{
			// Получение списка заказов
			var orders = await _orderRepository.GetOrdersAsync(date);
			return PartialView("OrdersTable", orders);
		}


		[HttpGet]
		public async Task<IActionResult> GetOrderDetails(int orderId)
		{
			var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order != null)
			{
				//Получаем все позиции заказа из связанной корзины               
				var cartItems = await _orderRepository.GetCartItemsByOrderIdAsync(orderId);
                return PartialView("~/Views/Order/OrderDetails.cshtml", order);
            }
			else
			{
				return NotFound(); // Вернем 404 Not Found, если заказ не найден
			}
		}
	}
}
