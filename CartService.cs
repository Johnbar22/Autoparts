using Shop_ex.Models;

namespace Shop_ex
{
    public class CartService
    {
        public List<CartItem> _cartItems { get; set; } // Хранение объектов в памяти

        public CartService()
        {
            _cartItems = new List<CartItem>();
        }

        public void AddToCart(CartItem  autoPart)

        {
            _cartItems.Add(autoPart);
        }

        public void RemoveFromCart(CartItem autoPart)
        {
            _cartItems.Remove(autoPart);
        }

        public void ClearCart()
        {
            _cartItems.Clear();
        }

        public List<CartItem> GetCartItems()
        {
            return _cartItems;
        }

        public void PlaceOrder()
        {
            // В этом методе происходит сохранение корзины в базу данных
            // Например, можно использовать ваш сервис или контекст базы данных
            // Например:
            // using (var context = new YourDbContext())
            // {
            //     var cart = new Cart { Items = _cartItems };
            //     context.Carts.Add(cart);
            //     context.SaveChanges();
            // }

            // После сохранения корзины, очистите список корзины в памяти
            ClearCart();
        }
    }
}
