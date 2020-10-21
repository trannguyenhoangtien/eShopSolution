using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Services
{
    public interface IUserApiClient
    {
        Task<ResponseResult<string>> Authenticate(LoginRequest request);

        Task<ResponseResult<PagedResult<UserVm>>> GetUserPagings(GetUserPagingRequest request);

        Task<ResponseResult<bool>> CreateUser(RegisterRequest request);

        Task<ResponseResult<bool>> UpdateUser(Guid id, UserUpdateRequest request);

        Task<ResponseResult<UserVm>> GetById(Guid id);

        Task<ResponseResult<bool>> Delete(Guid id);

        Task<ResponseResult<bool>> RoleAssign(Guid id, RoleAssignRequest request);
    }
}