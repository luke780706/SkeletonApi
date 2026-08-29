using System.ComponentModel.DataAnnotations;

namespace SkeletonApi.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Range(0.01,double.MaxValue)]
        public decimal Price { get; set; }
    }
}
