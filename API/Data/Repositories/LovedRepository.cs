using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class LovedRepository : ILovedRepository
    {
        private readonly StoreContext _context;

        public LovedRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<LovedDto>> GetLovedDto(string buyerId)
        {
            var loved = await GetLoved(buyerId);

            if (loved == null) return ResultDto<LovedDto>.NotFound("Loved items not found");

            return ResultDto<LovedDto>.Success(loved.MapLovedToDto());
        }

        public async Task<ResultDto<LovedDto>> AddItemToLoved(string buyerId, int productId)
        {
            var loved = await GetLoved(buyerId);

            if (loved == null) 
            {
                loved = new Loved { BuyerId = buyerId };
                _context.Loveds.Add(loved);
            };

            var product = await _context.Products.FindAsync(productId);

            if (product == null) return ResultDto<LovedDto>.BadRequest("Product Not Found");

            loved.AddItem(product);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return ResultDto<LovedDto>.Success(loved.MapLovedToDto());

            return ResultDto<LovedDto>.BadRequest("Problem saving item to loved");
        }

        public async Task<ResultDto> RemoveLovedItem(string buyerId, int productId)
        {
            var loved = await GetLoved(buyerId);

            if (loved == null) return ResultDto.NotFound("Loved items not found");

            loved.RemoveItem(productId);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return ResultDto.Success("Item removed");

            return ResultDto.BadRequest("Problem removing loved item");
        }

        private async Task<Loved> GetLoved(string buyerId)
        {
            return await _context.Loveds
                    .Include(i => i.Items)
                    .ThenInclude(p => p.Product)
                    .FirstOrDefaultAsync(x => x.BuyerId == buyerId);
        }
    }
}
