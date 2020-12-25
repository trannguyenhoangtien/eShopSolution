using eShopSolution.ApiIntegration;
using eShopSolution.Utilities.Constaints;
using eShopSolution.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Controllers
{
    public class CartController : Controller
    {
        IProductApiClient _productApiClient;
        public CartController(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id, string languageId)
        {
            var product = await _productApiClient.GetById(id, languageId);
            var cartSession = HttpContext.Session.GetString(SystemContants.CartSession);
            List<CartItemVm> currentCart;

            if (!string.IsNullOrEmpty(cartSession))
                currentCart = JsonConvert.DeserializeObject<List<CartItemVm>>(cartSession);
            else
                currentCart = new List<CartItemVm>();

            if (currentCart.Any(x=>x.ProductId == id))
            {
                //quantity = currentCart.First(x => x.ProductId == id).Quantity + quantity;
                currentCart.First(x => x.ProductId == id).Quantity++;
            }
            else
            {
                var cartItem = new CartItemVm()
                {
                    Image = product.ResultObj.ThumbnailImage,
                    ProductId = id,
                    Quantity = 1
                };
                
                currentCart.Add(cartItem);
            }
            
            HttpContext.Session.SetString(SystemContants.CartSession, JsonConvert.SerializeObject(currentCart));
            return Ok();
        }
    }
}
