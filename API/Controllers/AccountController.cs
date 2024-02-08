using API.Data.Repositories;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IBasketRepository _basketRepository;
        private readonly ILovedRepository _lovedRepository;
        private readonly TokenService _tokenService;

        public AccountController(IAccountRepository accountRepository, IBasketRepository basketRepository, ILovedRepository lovedRepository, TokenService tokenService)
        {
            _accountRepository = accountRepository;
            _basketRepository = basketRepository;
            _lovedRepository = lovedRepository;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var result = await _accountRepository.Login(loginDto);

            if (result.IsUnauthorized)
            {
                return Unauthorized();
            }

            var user = result.Data;

            var userBasket = await _basketRepository.RetrieveBasket(loginDto.Username);
            var anonBasket = await _basketRepository.RetrieveBasket(Request.Cookies["buyerId"]);

            var userLoved = await _lovedRepository.RetrieveLoved(loginDto.Username);
            var anonLoved = await _lovedRepository.RetrieveLoved(Request.Cookies["buyerId"]);

            if (anonBasket != null)
            {
                if (userBasket != null) _basketRepository.RemoveBasket(userBasket);
                await _basketRepository.TransferBasket(anonBasket, user.UserName);
                Response.Cookies.Delete("buyerId");
            }

            if (anonLoved != null)
            {
                if (userBasket != null) _lovedRepository.RemoveLoved(userLoved);
                await _lovedRepository.TransferLoved(anonLoved, user.UserName);
                Response.Cookies.Delete("buyerId");
            }

            return new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.GenerateToken(user),
                Basket = anonBasket != null ? anonBasket.MapBasketToDto() : userBasket?.MapBasketToDto(),
                Loved = anonLoved != null ? anonLoved.MapLovedToDto() : userLoved?.MapLovedToDto(),
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var user = new User { UserName = registerDto.Username, Email = registerDto.Email };

            var result =  await _accountRepository.CreateUser(registerDto, user);

            if (!result.Succeeded) 
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return ValidationProblem();
            }

            await _accountRepository.AddToRoleMember(user);

            return StatusCode(201);
        }

        [Authorize]
        [HttpGet("currentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _accountRepository.GetUser(User.Identity.Name);

            var userBasket = await _basketRepository.RetrieveBasket(User.Identity.Name);
            var userLoved = await _lovedRepository.RetrieveLoved(User.Identity.Name);

            return new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.GenerateToken(user),
                Basket = userBasket?.MapBasketToDto(),
                Loved = userLoved?.MapLovedToDto(),
            };
        }

        [Authorize]
        [HttpGet("savedAddress")]
        public async Task<ActionResult<UserAddress>> GetSavedAddress()
        {
            var userName = User.Identity.Name;

            return await _accountRepository.GetSavedAddress(userName);
        }

    }
}
