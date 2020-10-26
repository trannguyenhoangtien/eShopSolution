using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Services
{
    public interface IProductApiClient
    {
        Task<PagedResult<ProductVm>> GetProductPagings(GetProductPagingRequest request);

        Task<ResponseResult<bool>> CreateProduct(ProductCreateRequest request);

        Task<ResponseResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);

        Task<ResponseResult<ProductVm>> GetById(int id, string languageId);
    }
}