using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.ApiIntegration;
using eShopSolution.Utilities.Constaints;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
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
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
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
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _productApiClient.CreateProduct(request);
            if (result.ResultObj)
            {
                TempData["result"] = "Thêm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var product = (await _productApiClient.GetById(id, languageId)).ResultObj;
            if (product == null)
                return RedirectToAction("Index");

            var EditVm = new ProductUpdateRequest()
            {
                Description = product.Description,
                Details = product.Details,
                SeoTitle = product.SeoTitle,
                Name = product.Name,
                Id = product.Id,
                SeoAlias = product.SeoAlias,
                SeoDescription = product.SeoDescription,
                IsFeturead = product.IsFeturead,
            };
            return View(EditVm);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _productApiClient.UpdateProduct(request);
            if (result.ResultObj)
            {
                TempData["result"] = "Sửa thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> CategoryAssign(int id)
        {
            var categoryAssignRequest = await GetCategoryAssignRequest(id);
            return View(categoryAssignRequest);
        }

        [HttpPost]
        public async Task<IActionResult> CategoryAssign(CategoryAssignRequest request)
        {
            var roleAssignRequest = await GetCategoryAssignRequest(request.Id);
            if (!ModelState.IsValid)
                return View(roleAssignRequest);

            var result = await _productApiClient.CategoryAssign(request.Id, request);
            if (result.IsSuccess)
            {
                TempData["result"] = "Cập nhật danh mục sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);

            return View(roleAssignRequest);
        }

        private async Task<CategoryAssignRequest> GetCategoryAssignRequest(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var productObj = await _productApiClient.GetById(id, languageId);
            var categoryObj = await _categoryApiClient.GetAll(languageId);
            var roleAssignRequest = new CategoryAssignRequest();
            foreach (var cate in categoryObj.ResultObj)
            {
                roleAssignRequest.Categories.Add(new SelectItem()
                {
                    Id = cate.Id.ToString(),
                    Name = cate.Name,
                    Selected = productObj.ResultObj.Categories.Select(x => x.Name).Contains(cate.Name)
                });
            }
            return roleAssignRequest;
        }
    }
}