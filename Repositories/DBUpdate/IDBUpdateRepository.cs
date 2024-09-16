using Shop_ex.Models;

namespace Shop_ex.Repositories.DBUpdate
{
    public interface IDBUpdateRepository
    {
        Task<List<AutoParts>> GetAllAutoPartsAsync();
        Task<AutoParts> GetAutoPartByNameAsync(string name);
        Task UpdateAutoPartsRangeAsync(IEnumerable<AutoParts> autoParts);
        Task SaveChangesAsync();
    }
}
