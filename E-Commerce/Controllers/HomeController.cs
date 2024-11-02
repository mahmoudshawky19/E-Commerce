using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Repositery.InterfaceCategory;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace E_Commerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger; private readonly IProductRepository productRepository;

         public HomeController(ILogger<HomeController> logger , IProductRepository productRepository)
        {
            _logger = logger;  this.productRepository = productRepository;
        }

        public IActionResult Index()
        {
            var products = productRepository.GetAll();
            return View(model: products);
        }
   
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult NotFoundPage()
        {
            return View();
        }
        public IActionResult Details(int ProductId)
        {
            var product =    productRepository.GetOne([],e=>e.Id== ProductId); 
             if (product != null)
                return View(model: product);
            else
                return RedirectToAction("NotFoundPage");

        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
