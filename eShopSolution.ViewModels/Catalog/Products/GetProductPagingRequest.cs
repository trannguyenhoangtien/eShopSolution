﻿using eShopSolution.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Products
{
    public class GetProductPagingRequest : PagingRequestBase
    {
        public string LanguageId { get; set; }
        public string Keyword { get; set; }
        public List<int> CategoryIds { get; set; }
    }
}
