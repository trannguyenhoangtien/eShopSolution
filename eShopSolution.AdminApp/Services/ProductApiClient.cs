using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Services
{
    public class ProductApiClient : BaseApiClient, IProductApiClient
    {
        public ProductApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<PagedResult<ProductVm>> GetProductPagings(GetProductPagingRequest request)
        {
            string url = $"/api/products/paging?pageIndex={request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}&languageId={request.LanguageId}";
            return await GetAsync<PagedResult<ProductVm>>(url);
        }
    }
}