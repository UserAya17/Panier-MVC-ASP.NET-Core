using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Models;

namespace MyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
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

        [Route("[area]/products/{brand?}")]
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

            return View(products);
        }

        [HttpGet]
        public IActionResult Add(int? id)
        {
            if (id.HasValue)
            {
                var product = _context.Products.Find(id.Value);
                if (product != null)
                {
                    return View("AddEdit", product);
                }
            }
            return View("AddEdit", new Product());
        }

        [HttpPost]
        public IActionResult Add(Product product)
        {
            // Add operation if id is 0
            if (ModelState.IsValid)
            {
                if (product.Id == 0)
                {
                    _context.Products.Add(product);
                }
                else
                {
                    _context.Products.Update(product);
                }

                _context.SaveChanges();
                return RedirectToAction("List");
            }
            return View("AddEdit", product);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                return View(product);
            }
            return RedirectToAction("List");
        }

        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}
