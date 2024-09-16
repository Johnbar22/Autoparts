using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop_ex.Models;
using System.Security.Claims;
using Shop_ex.Repositories.UserRepository;
using Shop_ex.Repositories.Registration;


namespace Shop_ex.Controllers
{
	public class UserController : Controller
	{
		private readonly IUserRepository _userRepository;
		private readonly IRegistrationRepository _registrationRepository;

		public UserController(IUserRepository userRepository, IRegistrationRepository registrationRepository)
		{
			_userRepository = userRepository;
			_registrationRepository = registrationRepository;
		}
		public IActionResult Index()
		{
			var userRole = Convert.ToInt32(User.FindFirstValue(ClaimTypes.Role));			
			var name = User.FindFirstValue(ClaimTypes.GivenName);
			var phoneNumber = User.FindFirstValue(ClaimTypes.MobilePhone);
			var email = User.FindFirstValue(ClaimTypes.Email);
			ViewBag.Username = name;
			ViewBag.Telephone = phoneNumber;
			ViewBag.Email = email;
			ViewBag.UserRole = userRole;
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> GetOrders(DateTime? date)
		{
			var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var orders = await _userRepository.GetUserOrdersAsync(userId, date);
			return PartialView("~/Views/Order/OrdersTable.cshtml", orders);
		}
		public async Task<IActionResult> EditInfo(string Name, string Email, string Phone)
		{
			// Получение текущего пользователя
			var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var user = await _userRepository.GetUserByIdAsync(userId);

			if (user == null)
			{
				return NotFound();
			}

			// Проверка, используется ли новый email другим пользователем
			var existingUser = await _registrationRepository.GetUserByEmailAsync(Email);
			if (existingUser != null && existingUser.Id != user.Id)
			{
				ModelState.AddModelError("Email", "Этот адрес электронной почты уже используется другим пользователем.");
			}

			// Создание объекта пользователя для валидации
			var updatedUser = new User
			{
				Username = Name,
				Email = Email,
				Telephone = Phone,
				Orders = user.Orders,
				Password = user.Password
			};

			// Проверка валидности данных пользователя
			TryValidateModel(updatedUser);

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

			// Обновление полей пользователя
			user.Username = Name;
			user.Email = Email;
			user.Telephone = Phone.Replace(" ", "");

			// Сохранение изменений в базе данных
			await _userRepository.UpdateUserAsync(user);
			await UpdateUserClaims(user);

			return Json(new { success = true });
		}

		private async Task UpdateUserClaims(User user)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimsIdentity.DefaultRoleClaimType, user.RoleId.ToString()),
				new Claim(ClaimTypes.GivenName, user.Username),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.MobilePhone, user.Telephone?? "")
			};

			ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
				  ClaimsIdentity.DefaultRoleClaimType);

			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
		}

	}
}
