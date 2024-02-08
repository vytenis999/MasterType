using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface ILovedRepository
    {
        Task<ResultDto<LovedDto>> GetLoved(string buyerId);
        Task<ResultDto<LovedDto>> AddItemToLoved(string buyerId, int productId);
        Task<ResultDto> RemoveLovedItem(string buyerId, int productId);
        Task<Loved> RetrieveLoved(string buyerId);
        void RemoveLoved(Loved userLoved);
        Task TransferLoved(Loved userLoved, string userName);
    }
}
