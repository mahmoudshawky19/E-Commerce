namespace E_Commerce.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public ICollection<Product> products { get; set; } = new List<Product>();

    }
}
