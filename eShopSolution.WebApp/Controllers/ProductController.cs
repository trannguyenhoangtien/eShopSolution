using eShopSolution.ApiIntegration;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categoryApiClient;
        public ProductController(IProductApiClient productApiClient, ICategoryApiClient categoryApiClient)
        {
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
        }
        public IActionResult Detail(int id)
        {
            return View();
        }

        public async Task<IActionResult> Category(int id, string culture, int page = 1)
        {
            var products = await _productApiClient.GetProductPagings(new GetProductPagingRequest() {
                CategoryId = id,
                LanguageId = culture,
                PageIndex = 1,
                Keyword = "",
                PageSize = 12
            });
            return View(new ProductCategoryViewModel() { 
                Products = products,
                Category = (await _categoryApiClient.GetById(culture,id)).ResultObj
            });
        }
    }
}
