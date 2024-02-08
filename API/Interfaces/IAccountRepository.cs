using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface IAccountRepository
    {
        Task<ResultDto<User>> Login(LoginDto loginDto);
        Task<IdentityResult> CreateUser(RegisterDto registerDto, User user);
        Task AddToRoleMember(User user);
        Task<User> GetUser(string userName);
        Task<ActionResult<UserAddress>> GetSavedAddress(string userName);
    }
}
