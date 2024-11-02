using E_Commerce.Models;
 using E_Commerce.Repositery.InterfaceCategory;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace E_Commerce.Controllers
{
    public class CartController : Controller
    {
    private readonly  ICartRepository cartRepositery;
        private readonly UserManager<ApplicationUser> userManager;
        public CartController(ICartRepository cartRepositery , UserManager<ApplicationUser> userManager)
        {
            this.cartRepositery = cartRepositery; 
            this.userManager = userManager;
        }
        public IActionResult AddToCart(int count, int productId)
        {
            Cart cart = new Cart()
            {
                Count = count,
                ProductId = productId,
                ApplicationUserId = userManager.GetUserId(User)
            };

            cartRepositery.Add(cart);
            cartRepositery.Commit();

            TempData["success"] = "Add product to cart successfully";

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Index()
        {
    
            var ApplicationUserId = userManager.GetUserId(User);

            var cartProduct = cartRepositery.GetAll([e => e.Product], e => e.ApplicationUserId == ApplicationUserId).ToList();

            ViewBag.Total = cartProduct.Sum(e => e.Product.Price * e.Count);

            return View(cartProduct.ToList());
        }
        public IActionResult Increment(int productId)
        {
            var ApplicationUserId = userManager.GetUserId(User);

            var product = cartRepositery.GetOne(expression: e => e.ApplicationUserId == ApplicationUserId && e.ProductId == productId);

            if (product != null)
            {
                product.Count++;
                cartRepositery.Commit();

                return RedirectToAction("Index");
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
        public IActionResult Decrement(int productId)
        {
            var ApplicationUserId = userManager.GetUserId(User);

            var product = cartRepositery.GetOne(expression: e => e.ApplicationUserId == ApplicationUserId && e.ProductId == productId);

            if (product != null)
            {
                product.Count--;
                if (product.Count > 0)
               
                    cartRepositery.Commit();
              
                else
                    product.Count = 1;

                return RedirectToAction("Index");
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
        public IActionResult Delete(int productId)
        {
            var ApplicationUserId = userManager.GetUserId(User);

            var product = cartRepositery.GetOne(expression: e => e.ApplicationUserId == ApplicationUserId && e.ProductId == productId);

            if (product != null)
            {
                cartRepositery.Delete(product);
                cartRepositery.Commit();

                return RedirectToAction("Index");
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
        public IActionResult Pay()
        {
            var ApplicationUserId = userManager.GetUserId(User);

            var cartProduct = cartRepositery.GetAll([e => e.Product], e => e.ApplicationUserId == ApplicationUserId).ToList();

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/checkout/success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancel",
            };

            foreach (var item in cartProduct)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },
                        UnitAmount = (long)item.Product.Price * 100,
                    },
                    Quantity = item.Count,
                });
            }

            var service = new SessionService();
            var session = service.Create(options);
            return Redirect(session.Url);
        }
    }
}
