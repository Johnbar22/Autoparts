using Microsoft.EntityFrameworkCore;
using Shop_ex.Models;

namespace Shop_ex.Repositories.Home
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _context;

        public HomeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AutoParts>> SearchAutoPartsAsync(string searchString)
        {
            return await _context.AutoParts
                .Where(ap => (ap.Name.Contains(searchString) || ap.Code.Contains(searchString)) && ap.Count > 0)
                .ToListAsync();
        }
    }
}
