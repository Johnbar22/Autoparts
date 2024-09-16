using Shop_ex.Models;

namespace Shop_ex.Repositories.OrderRepository
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrdersAsync(DateTime? date);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<List<CartItem>> GetCartItemsByOrderIdAsync(int orderId);
    }
}
