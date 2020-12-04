using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
    public interface IProductApiClient
    {
        Task<PagedResult<ProductVm>> GetProductPagings(GetProductPagingRequest request);

        Task<ResponseResult<bool>> CreateProduct(ProductCreateRequest request);
        Task<ResponseResult<bool>> UpdateProduct(ProductUpdateRequest request);

        Task<ResponseResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);

        Task<ResponseResult<ProductVm>> GetById(int id, string languageId);

        Task<List<ProductVm>> GetFeaturedProducts(string languageId, int take);

        Task<List<ProductVm>> GetLastestProducts(string languageId, int take);
    }
}