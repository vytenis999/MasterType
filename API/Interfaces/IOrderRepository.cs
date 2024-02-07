using API.DTOs;
using API.Entities.OrderAggregate;

namespace API.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<OrderDto>> GetOrders(string user);
        Task<OrderDto> GetOrder(int id, string user);
        Task<ResultDto<int>> CreateOrder(CreateOrderDto orderDto, string user);
    }
}
