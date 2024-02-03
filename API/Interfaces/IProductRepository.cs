using API.DTOs;
using API.Entities;
using API.RequestHelpers;
using Microsoft.EntityFrameworkCore;

namespace API.Interfaces
{
    public interface IProductRepository
    {
        Task<PagedList<Product>> GetProducts(ProductParams productParams);
        Task<List<Product>> GetNewestProducts();
        Task<Product> GetProduct(int id);
        Task<FilterDto> GetFilters();
    }
}
