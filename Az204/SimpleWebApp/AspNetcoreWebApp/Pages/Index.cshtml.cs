using AspNetcoreWebApp.Models;
using AspNetcoreWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetcoreWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IProductsService _productService;

        public IndexModel(ILogger<IndexModel> logger, IProductsService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public List<Product> Products { get; private set; }

        public bool IsFeatureA { get; set; }

        public void OnGet()
        {
            IsFeatureA = _productService.IsFaetureA().Result;
            Products = _productService.GetProducts();
        }
    }
}