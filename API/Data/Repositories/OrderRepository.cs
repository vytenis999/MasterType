using API.DTOs;
using API.Entities.OrderAggregate;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Extensions;

namespace API.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly StoreContext _context;

        public OrderRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<List<OrderDto>> GetOrders(string user)
        {
            return await _context.Orders
                .ProjectOrderToOrderDto()
                .Where(x => x.BuyerId == user)
                .ToListAsync();
        }

        public async Task<OrderDto> GetOrder(int id, string user)
        {
            return await _context.Orders
                .ProjectOrderToOrderDto()
                .Where(x => x.BuyerId == user && x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<ResultDto<int>> CreateOrder(CreateOrderDto orderDto, string user)
        {
            var basket = await _context.Baskets
                .RetrieveBasketWithItems(user)
                .FirstOrDefaultAsync();

            if (basket == null) return ResultDto<int>.NotFound("Could not locate basket");
            var items = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var productItem = await _context.Products.FindAsync(item.ProductId);
                var itemOrdered = new ProductItemOrdered
                {
                    ProductId = productItem.Id,
                    Name = productItem.Name,
                    PictureUrl = productItem.PictureUrl,
                };
                var orderItem = new OrderItem
                {
                    ItemOrdered = itemOrdered,
                    Price = productItem.Price,
                    Quantity = item.Quantity
                };
                items.Add(orderItem);
                productItem.QuantityInStock -= item.Quantity;
            }

            var subtotal = items.Sum(item => item.Price * item.Quantity);
            var deliveryFee = subtotal > 1000 ? 0 : 500;

            var order = new Order
            {
                OrderItems = items,
                BuyerId = user,
                ShippingAddress = orderDto.ShippingAddress,
                Subtotal = subtotal,
                DeliveryFee = deliveryFee,
                PaymentIntentId = basket.PaymentIntentId,
            };

            _context.Orders.Add(order);
            _context.Baskets.Remove(basket);

            if (orderDto.SaveAddress)
            {
                var userGet = await _context.Users
                    .Include(a => a.Address)
                    .FirstOrDefaultAsync(x => x.UserName == user);

                var address = new UserAddress
                {
                    FullName = orderDto.ShippingAddress.FullName,
                    Address1 = orderDto.ShippingAddress.Address1,
                    City = orderDto.ShippingAddress.City,
                    Zip = orderDto.ShippingAddress.Zip,
                    Country = orderDto.ShippingAddress.Country,
                };
                userGet.Address = address;
            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return ResultDto<int>.Success(order.Id);

            return ResultDto<int>.BadRequest("Problem creating order");
        }
    }
}
