using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Models
{
    public class CartItemVm
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
    }
}
