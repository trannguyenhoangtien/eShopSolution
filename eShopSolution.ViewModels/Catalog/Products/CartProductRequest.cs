using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Products
{
    public class CartProductRequest
    {
        public string LanguageId { get; set; }
        public List<int> ProductIds { get; set; }
    }
}
