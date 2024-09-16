using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop_ex.Models;
using Shop_ex.Controllers;
using Shop_ex.Services;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using static NuGet.Packaging.PackagingConstants;
using Shop_ex.Repositories.UserRepository;
using Shop_ex.Repositories.LoginRepository;
using NuGet.Common;
using NuGet.Protocol;



namespace Shop_ex.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginRepository _loginRepository;
        public LoginController(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var userRole = Convert.ToInt32(User.FindFirstValue(ClaimTypes.Role));
            ViewBag.UserRole = userRole;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            int userRoleId = 0;

            if (ModelState.IsValid)
			{
                var user = await _loginRepository.GetUserByEmailAsync(model.Login);
                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
				{
					await Authenticate(user);
                    userRoleId = user.RoleId;
                    return Json(new { success = true, userRoleId });
                }
                else
				{
					ModelState.AddModelError("", "Учетная запись не найдена. Проверьте введенные данные.");
				}
			}
			var errors = ModelState.Where(x => x.Value.Errors.Any())
		                           .ToDictionary(
			kvp => kvp.Key,
			kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
		    );

			return Json(new { success = false, errors, userRoleId });
		}


		// Метод для выхода из учетной записи администратора
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Очищаем данные аутентификации администратора
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");

        }

        private async Task Authenticate(User user)
        {

			var claims = new List<Claim>
			{
				new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username),
				new Claim(ClaimsIdentity.DefaultRoleClaimType, user.RoleId.ToString()),
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.GivenName, user.Username),
	            new Claim(ClaimTypes.Email, user.Email),
	            new Claim(ClaimTypes.MobilePhone, user.Telephone ?? "")
			};  

			ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _loginRepository.GetUserByEmailAsync(email);
            EmailService email2 = new EmailService();
            
            if (user != null)
            {
                var emailList = new List<string>
                        {
                            user.Email,
                            "shtutgart22@mail.ru"
                        };
                // Генерация токена для сброса пароля
                var token = Guid.NewGuid().ToString();

                // Сохранение токена и даты истечения
                user.ResetPasswordToken = token;
                user.ResetPasswordTokenExpiry = DateTime.Now.AddHours(1);
                await _loginRepository.UpdateUserAsync(user);

                // Генерация ссылки для сброса пароля
                var resetLink = Url.Action("ResetPassword", "Login", new { token = user.ResetPasswordToken }, Request.Scheme);

                // Отправка email
                email2.SendMail(emailList, "Ссылка восстановления пароля: ", resetLink);
                return Ok();
            }
            return View();
        }

        public async Task<IActionResult> ResetPassword(CartOrderModel model)
        {
            if (model.ResetPassword.Token == null)
            {
                return View(model);
            }

            var user = await _loginRepository.GetUserByResetTokenAsync(model.ResetPassword.Token);
            if (user == null || user.ResetPasswordTokenExpiry < DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "Неверный или истекший токен.");
                return View(model);
            }
            // Хеширование нового пароля и обновление данных пользователя
            user.Password = BCrypt.Net.BCrypt.HashPassword(model.ResetPassword.NewPassword);
            user.ResetPasswordToken = null;
            user.ResetPasswordTokenExpiry = null;

            await _loginRepository.SaveChangesAsync();

            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Токен сброса пароля не предоставлен.");
            }
            CartOrderModel model = new CartOrderModel();
            model.ResetPassword.Token = token;

            return View(model);
        }
    }
}
