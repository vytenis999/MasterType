using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        [HttpGet(Name = "GetBasket")]
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            var buyerId = User.Identity?.Name ?? Request.Cookies["buyerId"];

            if (string.IsNullOrEmpty(buyerId))
            {
                Response.Cookies.Delete("buyerId");
                return null;
            }

            var result = await _basketRepository.GetBasket(buyerId);

            if (result.IsSuccess)
            {
                return result.Data;
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
                return BadRequest(new ProblemDetails { Title = "Problem getting basket" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> AddItemToBasket(int productId, int quantity)
        {
            var buyerId = User.Identity?.Name;
            if (string.IsNullOrEmpty(buyerId))
            {
                buyerId = Request.Cookies["buyerId"];
                if (string.IsNullOrEmpty(buyerId))
                {
                    buyerId = Guid.NewGuid().ToString();
                    var cookieOptions = new CookieOptions { IsEssential = true, Expires = DateTime.Now.AddDays(30) };
                    Response.Cookies.Append("buyerId", buyerId, cookieOptions);
                }
            }

            var result = await _basketRepository.AddItemToBasket(buyerId, productId, quantity);

            if (result.IsSuccess)
            {
                return CreatedAtRoute("GetBasket", result.Data);
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
                return BadRequest(new ProblemDetails { Title = "Problem saving item to basket" });
            }
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveBasketItem(int productId, int quantity)
        {
            var buyerId = User.Identity?.Name ?? Request.Cookies["buyerId"];

            if (string.IsNullOrEmpty(buyerId))
            {
                Response.Cookies.Delete("buyerId");
                return null;
            }

            var result = await _basketRepository.RemoveBasketItem(buyerId, productId, quantity);

            if (result.IsSuccess)
            {
                return Ok(result.SuccessMessage);
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
                return BadRequest(new ProblemDetails { Title = "Problem removing basket item" });
            }
        }
    }
}
