using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "mininum Length must be 3") ]
        [MaxLength(100, ErrorMessage = "maxinum Length must be 50") ]
        public string Name { get; set; }
        public ICollection<Product> products { get; set; } = new List<Product>();
    }
}
