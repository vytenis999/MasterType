using API.DTOs;
using API.Entities;
using API.RequestHelpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface IProductRepository
    {
        Task<PagedList<Product>> GetProducts(ProductParams productParams);
        Task<List<Product>> GetNewestProducts();
        Task<Product> GetProduct(int id);
        Task<FilterDto> GetFilters();
        Task<ResultDto<Product>> CreateProduct(CreateProductDto productDto);
        Task<ResultDto<Product>> UpdateProduct(UpdateProductDto productDto);
        Task<ResultDto> DeleteProduct(int id);
    }
}
