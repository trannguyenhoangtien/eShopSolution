using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Categories
{
    public interface ICategoryService
    {
        Task<ResponseResult<List<CategoryVm>>> GetAll(string languageId);
        Task<ResponseResult<CategoryVm>> GetById(string languageId, int id);
    }
}
