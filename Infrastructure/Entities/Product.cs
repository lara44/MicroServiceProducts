namespace Infrastructure.Entities
{
    public class Product
    {
        public int Id { get; set;}
        public string Name { get; set;} = null!;
        public int Price { get; set;}
    }
}