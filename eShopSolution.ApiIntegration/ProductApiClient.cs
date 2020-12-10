using eShopSolution.Utilities.Constaints;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
    public class ProductApiClient : BaseApiClient, IProductApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseResult<bool>> CategoryAssign(int id, CategoryAssignRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            return await PutAsync<ResponseResult<bool>>($"/api/products/{id}/categories", json);
        }

        public async Task<ResponseResult<bool>> CreateProduct(ProductCreateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", session);
            var languageId = _httpContextAccessor.HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);

            var requestContent = new MultipartFormDataContent();
            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "thumbnailImage", request.ThumbnailImage.FileName);
            }

            requestContent.Add(new StringContent(request.Price.ToString()), "Price");
            requestContent.Add(new StringContent(request.OriginalPrice.ToString()), "OriginalPrice");
            requestContent.Add(new StringContent(request.Stock.ToString()), "Stock");
            requestContent.Add(new StringContent(request.Name.ToString()), "Name");
            requestContent.Add(new StringContent(request.Description.ToString()), "Description");
            requestContent.Add(new StringContent(request.Details.ToString()), "Details");
            requestContent.Add(new StringContent(request.SeoDescription.ToString()), "SeoDescription");
            requestContent.Add(new StringContent(request.SeoTitle.ToString()), "SeoTitle");
            requestContent.Add(new StringContent(request.SeoAlias.ToString()), "SeoAlias");
            requestContent.Add(new StringContent(languageId), "LanguageId");

            var response = await client.PostAsync($"/api/products", requestContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new ResponseSuccessResult<bool>(response.IsSuccessStatusCode);
            }

            //return new ResponseErrorResult<bool>(body);
            return JsonConvert.DeserializeObject<ResponseErrorResult<bool>>(body);
        }

        public async Task<ResponseResult<ProductVm>> GetById(int id, string languageId)
        {
            string url = $"/api/products/{id}/{languageId}";
            return await GetAsync<ResponseResult<ProductVm>>(url);
        }

        public async Task<List<ProductVm>> GetFeaturedProducts(string languageId, int take)
        {
            string url = $"/api/products/featured/{languageId}/{take}";
            return await GetAsync<List<ProductVm>>(url);
        }

        public async Task<List<ProductVm>> GetLastestProducts(string languageId, int take)
        {
            string url = $"/api/products/lastest/{languageId}/{take}";
            return await GetAsync<List<ProductVm>>(url);
        }

        public async Task<PagedResult<ProductVm>> GetProductPagings(GetProductPagingRequest request)
        {
            string url = $"/api/products/paging?pageIndex={request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}&languageId={request.LanguageId}&categoryId={request.CategoryId}";
            return await GetAsyncNotAuthorize<PagedResult<ProductVm>>(url);
        }

        public async Task<ResponseResult<bool>> UpdateProduct(ProductUpdateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", session);
            var languageId = _httpContextAccessor.HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);

            var requestContent = new MultipartFormDataContent();
            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "thumbnailImage", request.ThumbnailImage.FileName);
            }

            requestContent.Add(new StringContent(request.Id.ToString()), "Id");
            requestContent.Add(new StringContent(request.Name.ToString()), "Name");
            requestContent.Add(new StringContent(request.Description.ToString()), "Description");
            requestContent.Add(new StringContent(request.Details.ToString()), "Details");
            requestContent.Add(new StringContent(request.SeoDescription.ToString()), "SeoDescription");
            requestContent.Add(new StringContent(request.SeoTitle.ToString()), "SeoTitle");
            requestContent.Add(new StringContent(request.SeoAlias.ToString()), "SeoAlias");
            requestContent.Add(new StringContent(languageId), "LanguageId");

            var response = await client.PutAsync($"/api/products", requestContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new ResponseSuccessResult<bool>(response.IsSuccessStatusCode);
            }

            //return new ResponseErrorResult<bool>(body);
            return JsonConvert.DeserializeObject<ResponseErrorResult<bool>>(body);
        }
    }
}