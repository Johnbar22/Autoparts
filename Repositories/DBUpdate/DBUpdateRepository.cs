using Microsoft.EntityFrameworkCore;
using Shop_ex.Models;

namespace Shop_ex.Repositories.DBUpdate
{
    public class DBUpdateRepository : IDBUpdateRepository
    {
        private readonly ApplicationDbContext _context;

        public DBUpdateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AutoParts>> GetAllAutoPartsAsync()
        {
            return await _context.AutoParts.ToListAsync();
        }

        public async Task<AutoParts> GetAutoPartByNameAsync(string name)
        {
            return await _context.AutoParts.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task UpdateAutoPartsRangeAsync(IEnumerable<AutoParts> autoParts)
        {
            _context.AutoParts.UpdateRange(autoParts);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
