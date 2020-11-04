using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    public interface IProductService
    {
        Task<ResponseResult<int>> Create(ProductCreateRequest request);

        Task<ResponseResult<int>> Update(ProductUpdateRequest request);

        Task<ResponseResult<int>> Delete(int productId);

        Task<ResponseResult<ProductVm>> GetById(int productId, string languageId);

        Task<bool> UpdatePrice(int productId, decimal newPrice);

        Task<bool> UpdateStock(int productId, int qty);

        Task AddViewCount(int productId);

        Task<PagedResult<ProductVm>> GetAllPaging(GetProductPagingRequest request);

        Task<int> AddImage(int productId, ProductImageCreateRequest request);

        Task<int> AddImages(int productId, List<IFormFile> files);

        Task<int> RemoveImage(int imageId);

        Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request);

        Task<List<ProductImageViewModel>> GetListImage(int productId);

        Task<ProductImageViewModel> GetImageById(int imageId);

        Task<PagedResult<ProductVm>> GetAllByCategoryId(GetClientProductPagingRequest request, string languageId);

        Task<ResponseResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);

        Task<List<ProductVm>> GetFeaturedProducts(string languageId, int take);
    }
}