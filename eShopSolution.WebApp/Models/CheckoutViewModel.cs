using eShopSolution.ViewModels.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Models
{
    public class CheckoutViewModel
    {
        public List<CartItemVm> CartItems { get; set; }
        public CheckoutRequest CheckoutVm { get; set; }
    }
}
