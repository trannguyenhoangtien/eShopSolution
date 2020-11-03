using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
    public class UserApiClient : BaseApiClient, IUserApiClient
    {
        public UserApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<ResponseResult<string>> Authenticate(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            return await PostAsync<ResponseResult<string>>("/api/Users/authenticate", json);
        }

        public async Task<ResponseResult<bool>> CreateUser(RegisterRequest request)
        {
            string json = JsonConvert.SerializeObject(request);
            return await PostAsync<ResponseResult<bool>>("/api/Users/register", json);
        }

        public async Task<ResponseResult<bool>> Delete(Guid id)
        {
            return await DeleteAsync<ResponseResult<bool>>($"/api/Users/{id}");
        }

        public async Task<ResponseResult<UserVm>> GetById(Guid id)
        {
            return await GetAsync<ResponseResult<UserVm>>($"/api/Users/{id}");
        }

        public async Task<ResponseResult<PagedResult<UserVm>>> GetUserPagings(GetUserPagingRequest request)
        {
            string url = $"/api/Users/paging?pageIndex={request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}";
            return await GetAsync<ResponseResult<PagedResult<UserVm>>>(url);
        }

        public async Task<ResponseResult<bool>> RoleAssign(Guid id, RoleAssignRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            return await PutAsync<ResponseResult<bool>>($"/api/users/{id}/roles", json);
        }

        public async Task<ResponseResult<bool>> UpdateUser(Guid id, UserUpdateRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            return await PutAsync<ResponseResult<bool>>($"/api/users/{id}", json);
        }
    }
}