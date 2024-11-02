 using E_Commerce.Models;
using E_Commerce.Repositery.InterfaceCategory;
using E_Commerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
 
namespace E_Commerce.Controllers
{
    [Authorize(Roles = $"{Sw.adminRole} , {Sw.CompanyRole}")]

    public class ProductController : Controller
    {
   private readonly     IProductRepository productRepository;

        private readonly ICategoryRepositery categoryRepositery; 

        public ProductController(IProductRepository productRepository, ICategoryRepositery categoryRepositery )
        {
            this.productRepository = productRepository; 
            this.categoryRepositery = categoryRepositery;

        }
        public IActionResult Index(int page = 1, string search = null)
        {
            int pageSize = 5;
            var totalProducts = productRepository.GetAll([]).Count();
            ;
            var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);  

            if (page <= 0) page = 1;  
            if (page > totalPages) page = totalPages;
            IQueryable<Product> pds = productRepository.GetAll([e => e.Category]);
            ;
            if (search != null)
            {
             search =    search.TrimStart();
               search = search.TrimEnd();
                pds = pds.Where(e => e.Name.Contains(search));
            }
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            if (pds.Any())
            {
                return View(model: pds.ToList());
            }
            return RedirectToAction( "NotFoundPage", "Home");
        }
        public IActionResult Create()
        {


           ViewBag.c = categoryRepositery.GetAll();
           
             return View(new Product());
        }
        [HttpPost]
        public IActionResult Create(Product product ,  IFormFile ImgUrl)
        {
            if (ModelState.IsValid)
            {
                if (ImgUrl.Length > 0) // 99656
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgUrl.FileName); // .png
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        ImgUrl.CopyTo(stream);
                    }

                    product.ImgUrl = fileName;
                }
                productRepository.Add(product);
                productRepository.Commit(); 
                 TempData["test"] = "created product successfuly";

                return RedirectToAction("");
            }
            ViewBag.c = categoryRepositery.GetAll();

            return View(product);
            
            }
        public IActionResult Edit(int productid)
        {
         var pd=    productRepository.GetAll().FirstOrDefault(e=>e.Id == productid);

           var c= categoryRepositery.GetAll();
            ViewBag.c = c;

         
            return View(pd);
        }
        [HttpPost]
        public IActionResult Edit(Product product ,IFormFile ImgUrl)

        {
            var oldproduct = productRepository.GetOne([], e => e.Id == product.Id, false);
              if (ModelState.IsValid)
            {
                if (oldproduct == null)
                {
                    return RedirectToAction(""); // أو أي إجراء آخر تراه مناسبًا
                }
                if (ImgUrl != null && ImgUrl.Length > 0) // 99656
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgUrl.FileName); // .png
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);
                    var oldfilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", oldproduct.ImgUrl);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        ImgUrl.CopyTo(stream);
                    }
                    if (System.IO.File.Exists(oldfilePath))
                    {
                        System.IO.File.Delete(oldfilePath);
                    }
                    product.ImgUrl = fileName;
                }
                else
                {
                    product.ImgUrl = oldproduct.ImgUrl;
                }
          
                productRepository.Edit(product);
                productRepository.Commit();
             TempData["test"] = "Edited product successfuly";
            return RedirectToAction(""); 
            }
            var c = categoryRepositery.GetAll();
            ViewBag.c = c;
            product.ImgUrl = oldproduct.ImgUrl;
            return View(product);
        }
        public IActionResult Delete(int productid)
        {
           var v =  productRepository.GetAll().FirstOrDefault(e => e.Id == productid);
             var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", v.ImgUrl);

            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }
            productRepository.Delete(v);
            productRepository.Commit();
  TempData["test"] = "Deleted product successfuly";

            return RedirectToAction("");
        }

    }
}
