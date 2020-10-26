using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.AdminApp.Services;
using eShopSolution.Utilities.Constaints;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace eShopSolution.AdminApp.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;
        private readonly ICategoryApiClient _categoryApiClient;

        public ProductController(IProductApiClient productApiClient
            , IConfiguration configuration, ICategoryApiClient categoryApiClient)
        {
            _productApiClient = productApiClient;
            _configuration = configuration;
            _categoryApiClient = categoryApiClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId, string keyword = "", int pageIndex = 1, int pageSize = 10)
        {
            var languageId = HttpContext.Session.GetString(SystemContains.AppSettings.DefaultLanguageId);
            var request = new GetProductPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = languageId,
                CategoryId = categoryId
            };

            var data = await _productApiClient.GetProductPagings(request);

            ViewBag.Keyword = keyword;
            var categories = await _categoryApiClient.GetAll(languageId);
            ViewBag.Categories = categories.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == x.Id
            });
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm]ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _productApiClient.CreateProduct(request);
            if (result)
            {
                TempData["result"] = "Thêm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm thất bại");
            return View(request);
        }
    }
}