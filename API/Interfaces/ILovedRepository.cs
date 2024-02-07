using API.DTOs;

namespace API.Interfaces
{
    public interface ILovedRepository
    {
        Task<ResultDto<LovedDto>> GetLoved(string buyerId);
        Task<ResultDto<LovedDto>> AddItemToLoved(string buyerId, int productId);
        Task<ResultDto> RemoveLovedItem(string buyerId, int productId);
    }
}
