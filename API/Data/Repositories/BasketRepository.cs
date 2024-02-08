using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly StoreContext _context;

        public BasketRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<BasketDto>> GetBasket(string buyerId)
        {
            var basket = await RetrieveBasket(buyerId);

            if (basket == null) return ResultDto<BasketDto>.NotFound("Basket not found");

            return ResultDto<BasketDto>.Success(basket.MapBasketToDto());
        }

        public async Task<ResultDto<BasketDto>> AddItemToBasket(string buyerId, int productId, int quantity)
        {
            var basket = await RetrieveBasket(buyerId);

            if (basket == null)
            {
                basket = new Basket { BuyerId = buyerId };
                _context.Baskets.Add(basket);
            }

            var product = await _context.Products.FindAsync(productId);

            if (product == null) return ResultDto<BasketDto>.BadRequest("Product Not Found");

            basket.AddItem(product, quantity);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return ResultDto<BasketDto>.Success(basket.MapBasketToDto());

            return ResultDto<BasketDto>.BadRequest("Problem saving item to basket");
        }

        public async Task<ResultDto> RemoveBasketItem(string buyerId,int productId, int quantity)
        {
            var basket = await RetrieveBasket(buyerId);

            if (basket == null) return ResultDto.NotFound("Basket not found");

            basket.RemoveItem(productId, quantity);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return ResultDto.Success("Item removed from the basket");

            return ResultDto.BadRequest("Problem removing basket item");
        }
        public async Task<Basket> RetrieveBasket(string buyerId)
        {
            if (string.IsNullOrEmpty(buyerId))
            {
                return null;
            }

            return await _context.Baskets
                .Include(i => i.Items)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(x => x.BuyerId == buyerId);
        }

        public void RemoveBasket(Basket userBasket)
        {
            _context.Baskets.Remove(userBasket);
        }

        public async Task TransferBasket(Basket userBasket, string userName)
        {
            userBasket.BuyerId = userName;

            await _context.SaveChangesAsync();
        }
    }
}
