using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
    public interface ICategoryApiClient
    {
        Task<ResponseResult<List<CategoryVm>>> GetAll(string languageId);
        Task<ResponseResult<CategoryVm>> GetById(string languageId, int id);
    }
}