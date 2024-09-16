using Shop_ex.Models;

namespace Shop_ex.Repositories.LoginRepository
{
    public interface ILoginRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task UpdateUserAsync(User user);
        Task<User> GetUserByResetTokenAsync(string token);
        Task SaveChangesAsync();
    }
}
