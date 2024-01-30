using API.DTOs;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class LovedExtensions
    {
        public static LovedDto MapLovedToDto(this Loved loved)
        {
            return new LovedDto
            {
                Id = loved.Id,
                BuyerId = loved.BuyerId,
                Items = loved.Items.Select(item => new LovedItemDto
                {
                    ProductId = item.ProductId,
                    Name = item.Product.Name,
                    Price = item.Product.Price,
                    PictureUrl = item.Product.PictureUrl,
                }).ToList()
            };
        }

        public static IQueryable<Loved> RetrieveLovedWithItems(this IQueryable<Loved> query, string buyerId)
        {
            return query.Include(i => i.Items).ThenInclude(p => p.Product).Where(b => b.BuyerId == buyerId);
        }
    }
}
