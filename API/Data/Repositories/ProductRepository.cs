using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.RequestHelpers;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;
        private readonly ImageService _imageService;

        public ProductRepository(StoreContext context, IMapper mapper, ImageService imageService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
        }
        public async Task<PagedList<Product>> GetProducts(ProductParams productParams)
        {
            var query = _context.Products
                .Sort(productParams.OrderBy)
                .Search(productParams.SearchTerm)
                .Filter(productParams.Brands, productParams.Types)
                .AsQueryable();

            var products = await PagedList<Product>.ToPagedList(query, productParams.PageNumber, productParams.PageSize);

            return products;
        }

        public async Task<List<Product>> GetNewestProducts()
        {
            var products = await _context.Products.OrderByDescending(p => p.Id).Take(6).ToListAsync();

            return products;
        }

        public async Task<Product> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            return product;
        }

        public async Task<FilterDto> GetFilters()
        {
            var brands = await _context.Products.Select(p => p.Brand).Distinct().ToListAsync();
            var types = await _context.Products.Select(p => p.Type).Distinct().ToListAsync();

            return new FilterDto {Brands = brands, Types = types };
        }

        public async Task<ResultDto<Product>> CreateProduct(CreateProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);

            if (productDto.File != null)
            {
                var imageResult = await _imageService.AddImageAsync(productDto.File);

                if (imageResult.Error != null) 
                    return ResultDto<Product>.BadRequest(imageResult.Error.Message);

                product.PictureUrl = imageResult.SecureUrl.ToString();
                product.PublicId = imageResult.PublicId;
            }

            _context.Products.Add(product);

            var result = await _context.SaveChangesAsync() > 0;

            if (result)
            {
                return ResultDto<Product>.Success(product);
            }

            return ResultDto<Product>.BadRequest("Problem saving product into database");
        }

        public async Task<ResultDto<Product>> UpdateProduct(UpdateProductDto productDto)
        {
            var product = await GetProduct(productDto.Id);

            if (product == null) 
                return ResultDto<Product>.NotFound("Product not found");

            _mapper.Map(productDto, product);

            if (productDto.File != null)
            {
                var imageResult = await _imageService.AddImageAsync(productDto.File);

                if (imageResult.Error != null) 
                    return ResultDto<Product>.BadRequest(imageResult.Error.Message);

                if (!string.IsNullOrEmpty(product.PublicId)) 
                    await _imageService.DeleteImageAsync(product.PublicId);

                product.PictureUrl = imageResult.SecureUrl.ToString();
                product.PublicId = imageResult.PublicId;
            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result) 
            {
                return ResultDto<Product>.Success(product);
            }

            return ResultDto<Product>.BadRequest("Problem updating product");
        }

        public async Task<ResultDto> DeleteProduct(int id)
        {
            var product = await GetProduct(id);

            if (product == null)
                return ResultDto.NotFound("Product not found");

            if (!string.IsNullOrEmpty(product.PublicId))
                await _imageService.DeleteImageAsync(product.PublicId);

            _context.Products.Remove(product);

            var result = await _context.SaveChangesAsync() > 0;

            if (result)
            {
                return ResultDto.Success("Product deleted");
            }

            return ResultDto.BadRequest("Problem deleting product");
        }
    }
}
