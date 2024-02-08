using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;
        private readonly StoreContext _context;

        public AccountRepository(UserManager<User> userManager, TokenService tokenService, StoreContext context)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _context = context;
        }

        public async Task<ResultDto<User>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return ResultDto<User>.Unauthorized();

            return ResultDto<User>.Success(user);
        }

        public async Task<IdentityResult> CreateUser(RegisterDto registerDto, User user)
        {
            return await _userManager.CreateAsync(user, registerDto.Password);
        }

        public async Task AddToRoleMember(User user)
        {
            await _userManager.AddToRoleAsync(user, "Member");
        }

        public async Task<User> GetUser(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            return user;
        }

        public async Task<ActionResult<UserAddress>> GetSavedAddress(string userName)
        {
            return await _userManager.Users
                .Where(x => x.UserName == userName)
                .Select(user => user.Address)
                .FirstOrDefaultAsync();
        }
    }
}
