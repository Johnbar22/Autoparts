using Microsoft.EntityFrameworkCore;
using Shop_ex.Models;
using Shop_ex.Repositories.UserRepository;

public class UserRepository : IUserRepository
{
	private readonly ApplicationDbContext _context;

	public UserRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<User> GetUserByIdAsync(int userId)
	{
		return await _context.Users.FindAsync(userId);
	}

	public async Task<List<Order>> GetUserOrdersAsync(int userId, DateTime? date)
	{
		IQueryable<Order> ordersQuery = _context.Order.Where(o => o.UserId == userId);

		if (date != null)
		{
			ordersQuery = ordersQuery.Where(o => o.OrderDate.Date == date.Value.Date);
		}

		return await ordersQuery.ToListAsync();
	}

	public async Task UpdateUserAsync(User user)
	{
		_context.Update(user);
		await _context.SaveChangesAsync();
	}
}
