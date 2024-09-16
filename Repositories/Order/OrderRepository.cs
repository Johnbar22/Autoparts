using Microsoft.EntityFrameworkCore;
using Shop_ex.Models;
using Shop_ex.Repositories.OrderRepository;

namespace Shop_ex.Repositories.OrderRepository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersAsync(DateTime? date)
        {
            IQueryable<Order> ordersQuery = _context.Order;
            if (date.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.OrderDate.Date == date.Value.Date);
            }

            return await ordersQuery.ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Order.Include(o => o.Cart).FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<List<CartItem>> GetCartItemsByOrderIdAsync(int orderId)
        {
            var order = await _context.Order.FindAsync(orderId);
            if (order == null)
            {
                return new List<CartItem>();
            }

            return await _context.CartItem
                                 .Include(ci => ci.AutoPart)
                                 .Where(ci => ci.CartId == order.CartId)
                                 .ToListAsync();
        }
    }
}
