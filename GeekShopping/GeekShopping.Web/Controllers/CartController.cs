using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public CartController(
            IProductService productService,
            ICartService cartService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await FindUserCart());
        }

        [Authorize]
        [HttpPost]
        [ActionName("ApplyCoupon")]
        public async Task<IActionResult> ApplyCoupon(CartViewModel model)
        {
            var token = await HttpContext.GetContextTokenAsync();
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value!;

            var response = await _cartService.ApplyCoupon(model, token);

            if (response)
                return RedirectToAction(nameof(CartIndex));

            return View();
        }

        [Authorize]
        [HttpPost]
        [ActionName("RemoveCoupon")]
        public async Task<IActionResult> RemoveCoupon()
        {
            var token = await HttpContext.GetContextTokenAsync();
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value!;

            var response = await _cartService.RemoveCoupon(userId, token);

            if (response)
                return RedirectToAction(nameof(CartIndex));

            return View();
        }

        public async Task<IActionResult> Remove (int id)
        {
            var token = await HttpContext.GetContextTokenAsync();
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value!;

            var response = await _cartService.RemoveFromCart(id, token);

            if (response)
                return RedirectToAction(nameof(CartIndex));

            return View();
        }

        private async Task<CartViewModel> FindUserCart()
        {
            var token = await HttpContext.GetContextTokenAsync();
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value!;

            var response = await _cartService.FindCartByUserId(userId, token);
            if (response?.CartHeader != null)
            {
                foreach (var cartDetail in response.CartDetails)
                {
                    response.CartHeader.PurchaseAmount += (cartDetail.Product.Price * cartDetail.Count);
                }
            }

            return response ?? new CartViewModel();
        }
    }
}
