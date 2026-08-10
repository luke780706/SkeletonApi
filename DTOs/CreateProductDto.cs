using System.ComponentModel.DataAnnotations;

namespace SkeletonApi.DTOs
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
