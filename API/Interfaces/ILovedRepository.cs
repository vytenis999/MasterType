using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface ILovedRepository
    {
        Task<ResultDto<LovedDto>> GetLovedDto(string buyerId);
        Task<ResultDto<LovedDto>> AddItemToLoved(string buyerId, int productId);
        Task<ResultDto> RemoveLovedItem(string buyerId, int productId);
    }
}
