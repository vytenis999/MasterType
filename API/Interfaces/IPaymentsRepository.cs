using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace API.Interfaces
{
    public interface IPaymentsRepository
    {
        Task<ResultDto<BasketDto>> CreateOrUpdatePaymentIntent(string user);
        Task<EmptyResult> StripeWebHook(Charge charge);
    }
}
