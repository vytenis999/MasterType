using API.Data;
using API.DTOs;
using API.Entities.OrderAggregate;
using API.Extensions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Reflection.Metadata.Ecma335;

namespace API.Data.Repositories
{
    public class PaymentsRepository : IPaymentsRepository
    {
        private readonly PaymentService _paymentService;
        private readonly StoreContext _context;
        private readonly IConfiguration _config;

        public PaymentsRepository(PaymentService paymentService, StoreContext context, IConfiguration config)
        {
            _paymentService = paymentService;
            _context = context;
            _config = config;
        }

        public async Task<ResultDto<BasketDto>> CreateOrUpdatePaymentIntent(string user)
        {
            var basket = _context.Baskets
                .RetrieveBasketWithItems(user)
                .FirstOrDefault();

            if (basket == null)
            {
                return ResultDto<BasketDto>.NotFound("Basket not found");
            }

            var intent = await _paymentService.CreateOrUpdatePaymentIntent(basket);

            if (intent == null)
            {
                return ResultDto<BasketDto>.BadRequest("Problem creating payment intent");
            }

            basket.PaymentIntentId = basket.PaymentIntentId ?? intent.Id;
            basket.ClientSecret = basket.ClientSecret ?? intent.ClientSecret;

            _context.Update(basket);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return ResultDto<BasketDto>.BadRequest("Problem updating basket with intent");
            }

            return ResultDto<BasketDto>.Success(basket.MapBasketToDto());
        }

        public async Task<EmptyResult> StripeWebHook(Charge charge)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x =>
                x.PaymentIntentId == charge.PaymentIntentId);

            if (charge.Status == "succeeded") order.OrderStatus = OrderStatus.PaymentReceived;

            await _context.SaveChangesAsync();

            return new EmptyResult();
        }


    }
}
