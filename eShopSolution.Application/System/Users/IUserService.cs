using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Users
{
    public interface IUserService
    {
        Task<ResponseResult<string>> Authenticate(LoginRequest request);

        Task<ResponseResult<bool>> Register(RegisterRequest request);

        Task<ResponseResult<bool>> Update(Guid id, UserUpdateRequest request);

        Task<ResponseResult<PagedResult<UserVm>>> GetUserPaging(GetUserPagingRequest request);

        Task<ResponseResult<UserVm>> GetById(Guid id);

        Task<ResponseResult<bool>> Delete(Guid id);

        Task<ResponseResult<bool>> RoleAssign(Guid id, RoleAssignRequest request);
    }
}