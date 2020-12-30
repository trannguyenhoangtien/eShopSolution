using eShopSolution.ApiIntegration;
using eShopSolution.Utilities.Constaints;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Sales;
using eShopSolution.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public async Task <IActionResult> Checkout()
        {
            return View(await GetCheckoutViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel request)
        {
            var model = await GetCheckoutViewModel();
            var orderDetals = new List<OrderDetailVm>();
            foreach (var item in model.CartItems)
            {
                orderDetals.Add(new OrderDetailVm()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }

            var checkoutRequest = new CheckoutRequest()
            {
                Address = request.CheckoutVm.Address,
                Name = request.CheckoutVm.Name,
                Email = request.CheckoutVm.Email,
                PhoneNumber = request.CheckoutVm.PhoneNumber,
                OrderDetails = orderDetals
            };
            //TODO: Add to API
            ViewBag.SuccessMsg = "Order successfully";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetListCart()
        {
            var languageId = CultureInfo.CurrentCulture.Name;
            var cartSession = HttpContext.Session.GetString(SystemContants.CartSession);
            List<CartItemVm> currentCart;

            if (!string.IsNullOrEmpty(cartSession))
            {
                currentCart = JsonConvert.DeserializeObject<List<CartItemVm>>(cartSession);
                var products = await _productApiClient.GetCartProducts(new CartProductRequest()
                {
                    LanguageId = languageId,
                    ProductIds = currentCart.Select(x => x.ProductId).ToList()
                });
                foreach (var item in currentCart)
                {
                    var product = products.FirstOrDefault(x=>x.Id == item.ProductId);
                    item.Name = product.Name;
                    item.Description = product.Description;
                    item.Price = product.Price;
                    item.Image = product.ThumbnailImage;
                }
            }
            else
                currentCart = new List<CartItemVm>();

            return Ok(currentCart);
        }

        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            //var product = await _productApiClient.GetById(id, languageId);
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
                    ProductId = id,
                    Quantity = 1
                };
                
                currentCart.Add(cartItem);
            }
            
            HttpContext.Session.SetString(SystemContants.CartSession, JsonConvert.SerializeObject(currentCart));
            return Ok();
        }

        [HttpPost]
        public IActionResult UpdateCart(int id, int quantity)
        {
            var cartSession = HttpContext.Session.GetString(SystemContants.CartSession);
            List<CartItemVm> currentCart;

            if (!string.IsNullOrEmpty(cartSession))
                currentCart = JsonConvert.DeserializeObject<List<CartItemVm>>(cartSession);
            else currentCart = new List<CartItemVm>();

            for (int i = 0; i < currentCart.Count; i++)
            {
                if (currentCart[i].ProductId == id)
                {
                    if (quantity <= 0)
                        currentCart.Remove(currentCart[i]);
                    else
                        currentCart[i].Quantity = quantity;
                }
            }

            HttpContext.Session.SetString(SystemContants.CartSession, JsonConvert.SerializeObject(currentCart));
            return Ok();
        }

        private async Task<CheckoutViewModel> GetCheckoutViewModel()
        {
            var languageId = CultureInfo.CurrentCulture.Name;
            var cartSession = HttpContext.Session.GetString(SystemContants.CartSession);
            List<CartItemVm> currentCart;

            if (!string.IsNullOrEmpty(cartSession))
            {
                currentCart = JsonConvert.DeserializeObject<List<CartItemVm>>(cartSession);
                var products = await _productApiClient.GetCartProducts(new CartProductRequest()
                {
                    LanguageId = languageId,
                    ProductIds = currentCart.Select(x => x.ProductId).ToList()
                });
                foreach (var item in currentCart)
                {
                    var product = products.FirstOrDefault(x => x.Id == item.ProductId);
                    item.Name = product.Name;
                    item.Description = product.Description;
                    item.Price = product.Price;
                    item.Image = product.ThumbnailImage;
                }
            }
            else
                currentCart = new List<CartItemVm>();

            return new CheckoutViewModel()
            {
                CartItems = currentCart,
                CheckoutVm = new CheckoutRequest(),
            };
        }
    }
}
