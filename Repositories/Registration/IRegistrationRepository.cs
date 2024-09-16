using Shop_ex.Models;
using Shop_ex.Repositories.Registration;

namespace Shop_ex.Repositories.Registration
{
    public interface IRegistrationRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<Role> GetRoleByNameAsync(string roleName);
        Task AddUserAsync(User user);
        Task SaveChangesAsync();
    }
}
