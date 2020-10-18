using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _config;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager
            , RoleManager<AppRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
        }

        public async Task<ResponseResult<string>> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return null;

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded)
                return null;

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Role, string.Join(";", roles)),
                new Claim(ClaimTypes.Name, string.Join(";", request.UserName)),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new ResponseSuccessResult<string>(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task<ResponseResult<UserVm>> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return new ResponseErrorResult<UserVm>("User không tồn tại");

            var userVm = new UserVm()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                Phone = user.PhoneNumber,
                UserName = user.UserName,
                DOB = user.DOB
            };

            return new ResponseSuccessResult<UserVm>(userVm);
        }

        public async Task<ResponseResult<PagedResult<UserVm>>> GetUserPaging(GetUserPagingRequest request)
        {
            var query = _userManager.Users.Where(x => request.Keyword == null || x.UserName.Contains(request.Keyword)
                    || x.PhoneNumber.Contains(request.Keyword));

            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new UserVm()
                {
                    Email = x.Email,
                    FirstName = x.FirstName,
                    Id = x.Id,
                    LastName = x.LastName,
                    Phone = x.PhoneNumber,
                    UserName = x.UserName
                }).ToListAsync();

            var pagedResult = new PagedResult<UserVm>()
            {
                TotalRecord = totalRow,
                Items = data
            };

            return new ResponseSuccessResult<PagedResult<UserVm>>(pagedResult);
        }

        public async Task<ResponseResult<bool>> Register(RegisterRequest request)
        {
            if (await _userManager.FindByNameAsync(request.UserName) != null)
            {
                return new ResponseErrorResult<bool>("Tài khoản đã tồn tại");
            }
            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                return new ResponseErrorResult<bool>("Email đã tồn tại");
            }

            var user = new AppUser()
            {
                DOB = request.DOB,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return new ResponseErrorResult<bool>(result.Errors.ToString());

            return new ResponseSuccessResult<bool>();
        }

        public async Task<ResponseResult<bool>> Update(Guid id, UserUpdateRequest request)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != id))
            {
                return new ResponseErrorResult<bool>("Email đã tồn tại");
            }

            var user = await _userManager.FindByIdAsync(id.ToString());
            user.DOB = request.DOB;
            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return new ResponseErrorResult<bool>(result.Errors.ToString());

            return new ResponseSuccessResult<bool>();
        }
    }
}