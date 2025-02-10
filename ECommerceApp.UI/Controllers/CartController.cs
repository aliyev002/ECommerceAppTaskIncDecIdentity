using ECommerce.Entities.Concrete;
using ECommerceApp.Business.Abstract;
using ECommerceApp.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.UI.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartSessionService _cartSessionService;
        private readonly ICartService _cartService;
        private readonly IProductService _productService;

        public CartController(ICartSessionService cartSessionService, ICartService cartService, IProductService productService)
        {
            _cartSessionService = cartSessionService;
            _cartService = cartService;
            _productService = productService;
        }

        public async Task<IActionResult> AddToCart(int productId, int page, int category)
        {
            var productToBeAdded = await _productService.GetByIdAsync(productId);
            var cart = _cartSessionService.GetCart();

            _cartService.AddToCart(cart, productToBeAdded);
            _cartSessionService.SetCart(cart);

            TempData.Add("message", $"Your Product , {productToBeAdded.ProductName} was added successfully.");

            return RedirectToAction("Index", "Product", new { page = page, category = category });
        }

        public IActionResult List()
        {
            var cart = _cartSessionService.GetCart();
            var model = new CartListViewModel
            {
                Cart = cart
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Complete()
        {
            var shippingDetailViewModel = new ShippingDetailsViewModel
            {
                ShippingDetails=new ShippingDetails()
            };
            return View(shippingDetailViewModel);   
        }

        [HttpPost]
        public IActionResult Complete(ShippingDetailsViewModel model)
        {
            if(!ModelState.IsValid) {
                return View(model);
            }
            TempData.Add("message", $"You {model.ShippingDetails.Firstname} your order is in progress.");
            return RedirectToAction("List");
        }

        
        public IActionResult IncreaseQuantity(int productId)
        {
            return UpdateQuantity(productId, 1);
        }
        private IActionResult UpdateQuantity(int productId, int change)
        {
            var cart = _cartSessionService.GetCart();
            var product = cart.CartLines.FirstOrDefault(c => c.Product.ProductId == productId);
            if (product != null)
            {
                var newQuantity = product.Quantity + change;
                if (newQuantity >= 1 && newQuantity <= product.Product.UnitsInStock)
                {
                    product.Quantity = newQuantity;
                    _cartSessionService.SetCart(cart);
                }
            }

            return RedirectToAction("List");
        }

        public IActionResult DecreaseQuantity(int productId)
        {
            return UpdateQuantity(productId, -1);
        }


    }
}
