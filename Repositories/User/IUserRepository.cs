using Shop_ex.Models;
using Shop_ex.Repositories.Registration;

namespace Shop_ex.Repositories.UserRepository
{
	public interface IUserRepository
	{
		Task<User> GetUserByIdAsync(int userId);
		Task<List<Order>> GetUserOrdersAsync(int userId, DateTime? date);
		Task UpdateUserAsync(User user);
	}
}
