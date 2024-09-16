namespace Shop_ex.Models
{
    public class AutoParts
    {
        public AutoParts(int id, string? name, string? code, int? price, int? count)
        {
            Id = id;
            Name = name;
            Code = code;
            Price = price;
            Count = count;
        }
        public AutoParts() { }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public int? Price { get; set; }
        public int? Count { get; set; }
    }
}
