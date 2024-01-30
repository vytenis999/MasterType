using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class LovedController : BaseApiController
    {
        private readonly StoreContext _context;

        public LovedController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetLoved")]
        public async Task<ActionResult<LovedDto>> GetLoved()
        {
            var loved = await RetrieveLoved(GetBuyerId());

            if (loved == null) return NotFound();

            return loved.MapLovedToDto();
        }

        [HttpPost]
        public async Task<ActionResult<LovedDto>> AddItemToLoved(int productId)
        {
            var loved = await RetrieveLoved(GetBuyerId());

            if (loved == null) loved = CreateLoved();

            var product = await _context.Products.FindAsync(productId);

            if (product == null) return BadRequest(new ProblemDetails { Title = "Product Not Found" });

            loved.AddItem(product);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetLoved", loved.MapLovedToDto());

            return BadRequest(new ProblemDetails { Title = "Problem saving item to loved" });
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveLovedItem(int productId)
        {
            var loved = await RetrieveLoved(GetBuyerId());

            if (loved == null) return NotFound();

            loved.RemoveItem(productId);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem removing loved item" });
        }

        private async Task<Loved> RetrieveLoved(string buyerId)
        {
            if (string.IsNullOrEmpty(buyerId))
            {
                Response.Cookies.Delete("buyerId");
                return null;
            }

            return await _context.Loveds
                .Include(i => i.Items)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(x => x.BuyerId == buyerId);
        }

        private string GetBuyerId()
        {
            return User.Identity?.Name ?? Request.Cookies["buyerId"];
        }

        private Loved CreateLoved()
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

            var loved = new Loved { BuyerId = buyerId };
            _context.Loveds.Add(loved);
            return loved;
        }
    }
}
