using ECommerceApp.Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.UI.Controllers
{
    //public class AdminController : Controller
    //{
    //    [Authorize(Roles ="Admin")]
    //    public IActionResult Index()
    //    {
    //        TempData["user"] = User.Identity.Name;
    //        return View();
    //    }
    //    [Authorize(Roles = "Admin,Editor")]
    //    public IActionResult Index2()
    //    {
    //        return View();
    //    }
    //}

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IProductService _productService;

        public AdminController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index(int page = 1, int category = 0)
        {
            TempData["user"] = User.Identity.Name;
            var items = await _productService.GetAllByCategoryId(category);
            var pageSize = 5;
            var model = new ProductListViewModel
            {
                Products = items.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                CurrentPage = page,
                PageSize = pageSize,
                PageCount = (int)Math.Ceiling(items.Count / (decimal)pageSize),
                CurrentCategory = category
            };
            return View(model);
        }

        public async Task<IActionResult> Remove(int productId, int page, int category)
        {
            //await _productService.DeleteAsync(productId);
            TempData["message"] = "Product deleted successfully";
            return RedirectToAction("Index", new { page = page, category = category });
        }
    }
}
