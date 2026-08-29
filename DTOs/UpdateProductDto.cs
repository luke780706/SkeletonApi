using System.ComponentModel.DataAnnotations;

namespace SkeletonApi.DTOs
{
    public class UpdateProductDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
