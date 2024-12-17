using Microsoft.AspNetCore.Mvc;
using MyShop.Models;
using Microsoft.AspNetCore.Http;

namespace MyShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ShopContext _context;
        private readonly List<ShoppingCartItem> _cartItems;

        public ShoppingCartController(ShopContext context)
        {
            _context = context;
            _cartItems = new List<ShoppingCartItem>();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddToCart(int id, int quantity)
        {
            // Trouver le produit à ajouter
            var productToAdd = _context.Products.Find(id);
            if (productToAdd == null)
            {
                return NotFound();
            }

            // Vérifier que la quantité demandée est inférieure ou égale au stock
            if (quantity > productToAdd.Stock)
            {
                TempData["ErrorMessage"] = "The quantity requested exceeds the available stock.";
                return RedirectToAction("Details", new { id = id });
            }

            // Récupérer le panier depuis la session ou créer une nouvelle liste
            var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("Cart") ?? new List<ShoppingCartItem>();

            // Vérifier si l'article existe déjà dans le panier
            var existingCartItem = cartItems.FirstOrDefault(item => item.Product.Id == id);

            if (existingCartItem != null)
            {
                // Si le produit existe déjà, augmenter la quantité
                if (existingCartItem.Quantity + quantity > productToAdd.Stock)
                {
                    TempData["ErrorMessage"] = "The total quantity in the cart exceeds the available stock.";
                    return RedirectToAction("Details", new { id = id });
                }

                existingCartItem.Quantity += quantity;
            }
            else
            {
                // Sinon, ajouter un nouvel élément au panier
                cartItems.Add(new ShoppingCartItem
                {
                    Product = productToAdd,
                    Quantity = quantity
                });
            }

            // Mettre à jour la session avec les nouveaux éléments du panier
            HttpContext.Session.Set("Cart", cartItems);

            // Écrire un message temporaire
            TempData["CartMessage"] = $"{productToAdd.Brand} {productToAdd.Model} added to the cart.";

            // Rediriger vers la vue du panier
            return RedirectToAction("ViewCart");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int id, int quantity, string operation)
        {
            var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("Cart") ?? new List<ShoppingCartItem>();
            var cartItem = cartItems.FirstOrDefault(item => item.Product.Id == id);

            if (cartItem == null)
            {
                return NotFound();
            }

            var product = _context.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            if (operation == "increment" && cartItem.Quantity < product.Stock)
            {
                cartItem.Quantity++;
            }
            else if (operation == "decrement" && cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
            }
            else if (operation != "increment" && operation != "decrement" && quantity >= 1 && quantity <= product.Stock)
            {
                cartItem.Quantity = quantity;
            }

            HttpContext.Session.Set("Cart", cartItems);

            return RedirectToAction("ViewCart");
        }

        public IActionResult ViewCart()
        {
            var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("Cart") ?? new List<ShoppingCartItem>();

            var cartViewModel = new ShoppingCartViewModel
            {
                CartItems = cartItems,
                TotalPrice = cartItems.Sum(item => item.Product.Price * item.Quantity)
            };
            ViewBag.CartMessage = TempData["CartMessage"];
            return View(cartViewModel);
        }

        public IActionResult RemoveItem(int id)
        {
            var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("Cart") ?? new List<ShoppingCartItem>();

            var itemToRemove = cartItems.FirstOrDefault(item => item.Product.Id == id);
            if (itemToRemove != null)
            {
                TempData["CartMessage"] = $"{itemToRemove.Product.Brand} {itemToRemove.Product.Model} removed from the cart";

                if (itemToRemove.Quantity > 1)
                {
                    itemToRemove.Quantity--;
                }
                else
                {
                    cartItems.Remove(itemToRemove);
                }
            }

            HttpContext.Session.Set("Cart", cartItems);

            return RedirectToAction("ViewCart");
        }
    }
}
