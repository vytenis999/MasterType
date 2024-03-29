﻿using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
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

        public async Task<ResultDto<LovedDto>> GetLoved(string buyerId)
        {
            var loved = await RetrieveLoved(buyerId);

            if (loved == null) return ResultDto<LovedDto>.NotFound("Loved items not found");

            return ResultDto<LovedDto>.Success(loved.MapLovedToDto());
        }

        public async Task<ResultDto<LovedDto>> AddItemToLoved(string buyerId, int productId)
        {
            var loved = await RetrieveLoved(buyerId);

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
            var loved = await RetrieveLoved(buyerId);

            if (loved == null) return ResultDto.NotFound("Loved items not found");

            loved.RemoveItem(productId);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return ResultDto.Success("Item removed");

            return ResultDto.BadRequest("Problem removing loved item");
        }

        public async Task<Loved> RetrieveLoved(string buyerId)
        {
            if (string.IsNullOrEmpty(buyerId))
            {
                return null;
            }

            return await _context.Loveds
                .Include(i => i.Items)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(x => x.BuyerId == buyerId);
        }

        public void RemoveLoved(Loved userLoved)
        {
            _context.Loveds.Remove(userLoved);
        }

        public async Task TransferLoved(Loved userLoved, string userName)
        {
            userLoved.BuyerId = userName;

            await _context.SaveChangesAsync();
        }
    }
}
