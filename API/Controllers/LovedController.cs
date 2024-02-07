using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class LovedController : BaseApiController
    {
        private readonly StoreContext _context;
        private readonly ILovedRepository _lovedRepository;

        public LovedController(StoreContext context, ILovedRepository lovedRepository)
        {
            _context = context;
            _lovedRepository = lovedRepository;
        }

        [HttpGet(Name = "GetLoved")]
        public async Task<ActionResult<LovedDto>> GetLoved()
        {
            var buyerId = User.Identity?.Name ?? Request.Cookies["buyerId"];

            if (string.IsNullOrEmpty(buyerId))
            {
                Response.Cookies.Delete("buyerId");
                return null;
            }

            var result = await _lovedRepository.GetLovedDto(buyerId);

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
                return BadRequest(new ProblemDetails { Title = "Problem getting item" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<LovedDto>> AddItemToLoved(int productId)
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

            var result = await _lovedRepository.AddItemToLoved(buyerId, productId);

            if (result.IsSuccess)
            {
                return CreatedAtRoute("GetLoved", result.Data);
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
                return BadRequest(new ProblemDetails { Title = "Problem saving item to loved" });
            }
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveLovedItem(int productId)
        {
            var buyerId = User.Identity?.Name ?? Request.Cookies["buyerId"];

            if (string.IsNullOrEmpty(buyerId))
            {
                Response.Cookies.Delete("buyerId");
                return null;
            }

            var result = await _lovedRepository.RemoveLovedItem(buyerId, productId);

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
                return BadRequest(new ProblemDetails { Title = "Problem removing loved item" });
            }
        }
    }
}
