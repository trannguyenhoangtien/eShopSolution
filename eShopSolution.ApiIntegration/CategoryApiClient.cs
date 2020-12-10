using eShopSolution.ApiIntegration;
using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
    public class CategoryApiClient : BaseApiClient, ICategoryApiClient
    {
        public CategoryApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<ResponseResult<List<CategoryVm>>> GetAll(string languageId)
        {
            return await GetAsyncNotAuthorize<ResponseResult<List<CategoryVm>>>($"/api/categories?languageId={languageId}");
        }

        public async Task<ResponseResult<CategoryVm>> GetById(string languageId, int id)
        {
            return await GetAsyncNotAuthorize<ResponseResult<CategoryVm>>($"/api/categories/{id}/{languageId}");
        }
    }
}