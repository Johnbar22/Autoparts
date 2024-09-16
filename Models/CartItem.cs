namespace Shop_ex.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int AutoPartId { get; set; }
        public  int Quantity { get; set; }
        public AutoParts AutoPart { get; set; }
        public virtual int CartId { get; set; }
        public virtual Cart? Cart { get; set; }

        public override string ToString()
        {
            return $"Наименование: {AutoPart.Name}. Цена: {AutoPart.Price}руб/шт. x {Quantity}шт. = {AutoPart.Price * Quantity} руб.\n";
        }
    }


}

