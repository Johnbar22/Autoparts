using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop_ex.Models;
using Shop_ex.Repositories.Registration;
using Shop_ex.Repositories.UserRepository;
using System;
using System.Security.Claims;

namespace Shop_ex.Controllers
{
    public class RegistrationController : Controller
    {
        private IRegistrationRepository _registrationRepositor;

        public RegistrationController(IRegistrationRepository context)
        {
            _registrationRepositor = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
                if (ModelState.IsValid)
            {
                User user = await _registrationRepositor.GetUserByEmailAsync(model.Email);

                if (user == null)
                {
                    // Хешируем пароль перед сохранением
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

                    // добавляем пользователя в бд
                    user = new User { Username = model.Username, Email = model.Email, Password = hashedPassword, Telephone = model.Telephone, };
                    Role userRole = await _registrationRepositor.GetRoleByNameAsync("user");
                    if (userRole != null)
                    {
                        user.Role = userRole;
                    }

                    await _registrationRepositor.AddUserAsync(user);
                    await _registrationRepositor.SaveChangesAsync();
                    await Authenticate(user); // аутентификация
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Email", "Пользователь с таким адресом электронной почты уже существует.");
                }

            }
            // Если модель не валидна или пользователь уже существует, возвращаем форму с ошибками
            var errors = ModelState
                    .Where(x => x.Value.Errors.Any())
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

            return Json(new { success = false, errors });
        }

        private async Task Authenticate(User user)
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
