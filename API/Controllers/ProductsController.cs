using API.Data;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.RequestHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<Product>>> GetProducts([FromQuery]ProductParams productParams) 
        {
            var products = await _productRepository.GetProducts(productParams);

            Response.AddPaginationHeader(products.MetaData);

            return products;
        }

        [HttpGet("newestProducts")]
        public async Task<ActionResult<List<Product>>> GetNewestProducts()
        {
            var products = await _productRepository.GetNewestProducts();

            return products;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id) 
        {
            var product = await _productRepository.GetProduct(id);

            if (product == null) { return NotFound(); }

            return product; 
        }

        [HttpGet("filters")]
        public async Task<IActionResult> GetFilters()
        {
            var filters = await _productRepository.GetFilters();

            return Ok(filters);
        }
    }
}
