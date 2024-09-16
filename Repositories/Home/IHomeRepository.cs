using Shop_ex.Models;

namespace Shop_ex.Repositories.Home
{
    public interface IHomeRepository
    {
        Task<List<AutoParts>> SearchAutoPartsAsync(string searchString);
    }
}
