using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace API.Controllers
{
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentsRepository _paymentsRepository;
        private readonly IConfiguration _config;

        public PaymentsController(IPaymentsRepository paymentsRepository, IConfiguration config)
        {
            _paymentsRepository = paymentsRepository;
            _config = config;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent()
        {
            var user = User.Identity.Name;

            var result = await _paymentsRepository.CreateOrUpdatePaymentIntent(user);

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            if (result.ErrorMessage == "Basket not found")
            {
                return NotFound(result.ErrorMessage);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"],
                _config["StripeSettings:WhSecret"]);

            var charge = (Charge)stripeEvent.Data.Object;

            var result = await _paymentsRepository.StripeWebHook(charge);

            return result;
        }
    }
}
