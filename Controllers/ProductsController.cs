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
        public ActionResult<IEnumerable<ProductDto>> GetAll()
        {
            var result = _productService.GetAll()
                                        .Select(x => new ProductDto
                                        {
                                            Id = x.Id,
                                            Name = x.Name,
                                            Price = x.Price
                                        });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<ProductDto> GetById(int id)
        {
            var singleProduct = _productService.GetById(id);

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
        public ActionResult<ProductDto> AddProduct(CreateProductDto productDto)
        {
            Product productToAdd = new()
            {
                Name = productDto.Name,
                Price = productDto.Price
            };

            var newProduct = _productService.AddProduct(productToAdd);

            ProductDto result = new()
            {
                Id = newProduct.Id,
                Name = newProduct.Name,
                Price = newProduct.Price
            };

            return CreatedAtAction(nameof(GetById), new { id = newProduct.Id }, result);
        }

        [HttpPut("{id}")]
        public ActionResult<ProductDto> UpdateProduct(int id, CreateProductDto productDto)
        {
            Product product = new()
            {
                Name = productDto.Name,
                Price = productDto.Price
            };

            var updatedProduct = _productService.Update(id, product);

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
        public IActionResult DeleteProduct(int id)
        {
            var deleted = _productService.Delete(id);

            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
