using Microsoft.EntityFrameworkCore;
namespace Shop_ex.Models;
public class Cart
{
    public int Id { get; set; }
    public List<CartItem>? Items { get; set; }
    public int? TotalPrice { get; set; } = 0;
}