using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop_ex.Models;
using Shop_ex.Repositories.Home;
using System.Diagnostics;
using System.Security.Claims;

namespace Shop_ex.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeRepository _homeRepository;
        public HomeController(IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
        }

        public IActionResult Index()
        {
            var userRole= Convert.ToInt32(User.FindFirstValue(ClaimTypes.Role));
            ViewBag.UserRole = userRole;
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Search(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return View("Index", null);
            }
            List<AutoParts> autoParts = await _homeRepository.SearchAutoPartsAsync(searchString);
            return PartialView("Search", autoParts);
        }
    }
}