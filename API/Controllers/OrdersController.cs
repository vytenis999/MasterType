using API.Data;
using API.DTOs;
using API.Entities;
using API.Entities.OrderAggregate;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Climate;

namespace API.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly StoreContext _context;
        private readonly IOrderRepository _orderRepository;

        public OrdersController(StoreContext context, IOrderRepository orderRepository)
        {
            _context = context;
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetOrders()
        {
            var user = User.Identity.Name;
            return await _orderRepository.GetOrders(user);
        }

        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var user = User.Identity.Name;
            return await _orderRepository.GetOrder(id,user);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateOrder(CreateOrderDto orderDto)
        {
            var user = User.Identity.Name;
            var result = await _orderRepository.CreateOrder(orderDto, user);

            if (result.IsSuccess)
            {
                return CreatedAtRoute("GetOrder", new { id = result.Data }, result.Data);
            }
            else if (result.IsNotFound)
            {
                return NotFound(result.ErrorMessage);
            }
            else if (result.IsBadRequest)
            {
                return BadRequest(result.ErrorMessage);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
