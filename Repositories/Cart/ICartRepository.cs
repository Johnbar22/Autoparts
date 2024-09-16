using Microsoft.EntityFrameworkCore.Storage;
using Shop_ex.Models;

namespace Shop_ex.Repositories.CartRepository
{
    public interface ICartRepository
    {
        Task<Cart> GetCartAsync(int cartId);
        Task<Cart> CreateCartAsync();
        Task UpdateCartAsync(Cart cart);
        Task<Order> CreateOrderAsync(Order order);
        Task<AutoParts> GetAutoPartAsync(int autoPartId);
        Task UpdateCartItemAsync(int cartItemId, int quantity);
        Task AddCartItemAsync(Cart cart, CartItem cartItem);
        Task<int> RemoveCartItemAsync(int cartItemId);
        Task<int> GetOrderCountAsync();
        Task<User> GetUserAsync(int userId);
        Task SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task UpdateAutoPartAsync(AutoParts autoPart);

    }

}
