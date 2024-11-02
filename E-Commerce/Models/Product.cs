using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "mininum Length must be 3")]
        [MaxLength(100, ErrorMessage = "maxinum Length must be 50")]

        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
      [Range(0,1000000)]
        public decimal Price { get; set; }
        [ValidateNever]

        public string ImgUrl { get; set; }
        [Range(0,5)]
        public double Rate { get; set; }
        [ValidateNever]
        public Category Category { get; set; }
        [ValidateNever]
        public Company Company { get; set; }
        public int? CompanyId { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
