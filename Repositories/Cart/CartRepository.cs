using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Shop_ex.Models;
using System.Data;

namespace Shop_ex.Repositories.CartRepository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetCartAsync(int cartId)
        {
            return await _context.Cart
                                 .Include(c => c.Items)
                                 .ThenInclude(ci => ci.AutoPart)
                                 .FirstOrDefaultAsync(c => c.Id == cartId);
        }

        public async Task<Cart> CreateCartAsync()
        {
            var cart = new Cart { Items = new List<CartItem>() };
            _context.Cart.Add(cart);
            await SaveChangesAsync();
            return cart;
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            _context.Cart.Update(cart);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _context.Order.AddAsync(order);
            return order;
        }

        public async Task<AutoParts> GetAutoPartAsync(int autoPartId)
        {
            return await _context.AutoParts.FindAsync(autoPartId);
        }

        public async Task UpdateCartItemAsync(int cartItemId, int quantity)
        {
            var cartItem = await _context.CartItem.FirstOrDefaultAsync(ci => ci.Id == cartItemId);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                _context.CartItem.Update(cartItem);
                await SaveChangesAsync();
            }
        }

        public async Task AddCartItemAsync(Cart cart, CartItem cartItem)
        {
            cartItem.CartId = cart.Id;
            _context.CartItem.Add(cartItem);
            await SaveChangesAsync();
        }

        public async Task<int> RemoveCartItemAsync(int cartItemId)
        {
            var cartItem = await _context.CartItem.FindAsync(cartItemId);
            if (cartItem != null)
            {
                _context.CartItem.Remove(cartItem);
                await SaveChangesAsync();

                var cart = await _context.Cart.Include(c => c.Items)
                                              .FirstOrDefaultAsync(c => c.Id == cartItem.CartId);
                return cart?.Items.Count ?? 0;
            }
            return 0;
        }

        public async Task<int> GetOrderCountAsync()
        {
            return await _context.Order.CountAsync();
        }

        public async Task<User> GetUserAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        }
        public async Task UpdateAutoPartAsync(AutoParts autoPart)
        {
            _context.AutoParts.Update(autoPart);
            await _context.SaveChangesAsync();
        }
    }
}
