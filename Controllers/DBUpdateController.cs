using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using Shop_ex.Models;
using Shop_ex.Repositories.DBUpdate;
using Shop_ex.Services;
using System.Text;


namespace Shop_ex.Controllers
{
    public class DBUpdateController : Controller
    {
        private readonly IDBUpdateRepository _context;
        public DBUpdateController(IDBUpdateRepository context)
        {
            _context = context;
        }

        [Authorize(Roles = "1")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<AutoParts> products;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("UTF-8");
            string filePath1 = "wwwroot\\Цена.xls";
            string filePath2 = "wwwroot\\Остатки.xls";
            ReadXLS readXLS = new ReadXLS();
            var autopart = readXLS.ReadExcelFile(filePath2);
            products = readXLS.ReadExcelFile(filePath1, autopart);
            List<AutoParts> dbAutoparts = await _context.GetAllAutoPartsAsync();

            var uniqueProducts = products.Concat(dbAutoparts)
                                   .GroupBy(p => p.Name)
                                   .Where(g => g.Count() == 1)
                                   .SelectMany(g => g);

            foreach (var product in products)
            {
                var match = _context.GetAutoPartByNameAsync(product.Name);
                if (match != null && (await match).Count != product.Count)
                {
                    (await match).Count = product.Count;
                }
            }

            await _context.UpdateAutoPartsRangeAsync(uniqueProducts);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Order");
        }
    }
}
