using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Products
{
    public class GetProductPagingRequest : PagingRequestBase
    {
        public string LanguageId { get; set; }
        public string Keyword { get; set; }
        public int? CategoryId { get; set; }
    }
}
