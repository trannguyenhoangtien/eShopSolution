using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Utilities.Slides;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Utility.Slides
{
    public interface ISlideService
    {
        Task<List<SlideVm>> GetAll();
    }
}