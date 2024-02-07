using API.DTOs;

namespace API.Interfaces
{
    public interface IBasketRepository
    {
        Task<ResultDto<BasketDto>> GetBasket(string buyerId);
        Task<ResultDto<BasketDto>> AddItemToBasket(string buyerId, int productId, int quantity);
        Task<ResultDto> RemoveBasketItem(string buyerId, int productId, int quantity);
    }
}
