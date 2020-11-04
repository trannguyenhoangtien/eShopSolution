using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Utilities.Slides
{
    public class SlideVm
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Url { set; get; }
        public int SortOrder { set; get; }
        public string Image { get; set; }
    }
}