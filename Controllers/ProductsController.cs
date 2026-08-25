using Microsoft.AspNetCore.Mvc;
using SkeletonApi.DTOs;
using SkeletonApi.Models;
using SkeletonApi.Services;

namespace SkeletonApi.Controllers
{
    /// <summary>
    /// Controller obsługujący operacje CRUD dla zasobów typu Product.
    /// Udostępnia operacje za pomocą metod HTTP GET, POST, PUT i DELETE.
    /// </summary>

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            var products = await _productService.GetAllAsync();

            var result = products.Select(x => new ProductDto
                                        {
                                            Id = x.Id,
                                            Name = x.Name,
                                            Price = x.Price
                                        });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {

            var singleProduct = await _productService.GetByIdAsync(id);

            if (singleProduct == null)
            {
                return NotFound();
            }

            ProductDto result = new ProductDto
            {
                Id = singleProduct.Id,
                Name = singleProduct.Name,
                Price = singleProduct.Price
            };

            return Ok(result);

        }
        [HttpPost]
        public async Task<ActionResult<ProductDto>> AddProduct(CreateProductDto productDto)
        {
            CreateProductModel productToAdd = new()
            {
                Name = productDto.Name,
                Price = productDto.Price
            };

            var newProduct = await _productService.AddProductAsync(productToAdd);

            ProductDto result = new()
            {
                Id = newProduct.Id,
                Name = newProduct.Name,
                Price = newProduct.Price
            };

            return CreatedAtAction(nameof(GetById), new { id = newProduct.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, UpdateProductDto productDto)
        {
            CreateProductModel product = new()
            {
                Name = productDto.Name,
                Price = productDto.Price
            };

            var updatedProduct = await _productService.UpdateAsync(id, product);

            if (updatedProduct == null)
            {
                return NotFound();
            }

            ProductDto result = new()
            {
                Id = updatedProduct.Id,
                Name = updatedProduct.Name,
                Price = updatedProduct.Price
            };

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleted = await _productService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
