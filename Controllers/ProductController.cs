using Microsoft.AspNetCore.Mvc;
using MyShop.Models;

namespace MyShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ShopContext _context;

        public ProductController(ShopContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("products/{brand?}")]
        public IActionResult List(string brand = "all")
        {
            List<Product> products;

            if (brand.Equals("all"))
            {
                products = _context.Products.ToList();
            }
            else
            {
                products = _context.Products.Where(p => p.Brand == brand).ToList();
            }

            // Prepend the image path for each product
            foreach (var product in products)
            {
                if (!string.IsNullOrEmpty(product.ImageFileName))
                {
                    product.ImageFileName = $"/images/{product.ImageFileName}";
                }
            }

            return View(products);
        }

        [Route("product/{id}/{slug?}")]
        public IActionResult Details(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null && product.ImageFileName != null)
            {
                // Correctly prepend the path to the image
                product.ImageFileName = $"/images/{product.ImageFileName}";
            }
            return View(product);
        }
    }
}
