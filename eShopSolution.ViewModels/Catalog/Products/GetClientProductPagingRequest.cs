using eShopSolution.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Products
{
    public class GetClientProductPagingRequest : PagingRequestBase
    {
        public int? CategoryId { get; set; }
    }
}
