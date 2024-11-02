using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Repositery;
using E_Commerce.Repositery.InterfaceCategory;
using E_Commerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    [Authorize(Roles =$"{Sw.adminRole} , {Sw.CompanyRole}")]
    public class CategoryController : Controller
    {
        ICategoryRepositery categoryrepositery ;  // new CategoryRepository();
        public CategoryController(ICategoryRepositery categoryrepositery)
        {
            this.categoryrepositery = categoryrepositery;
        }
        public IActionResult Index()
        {
            var category = categoryrepositery.GetAll([e=>e.products]);
            return View(model:category.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            Category category = new Category();
            return View(category);

        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                
                categoryrepositery.Add(category);
                categoryrepositery.Commit();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [HttpGet]
        public IActionResult Edit(int categoryid)
        {
         var cat = categoryrepositery.GetOne([],e=>e.Id ==categoryid);
                 return View(model: cat);
         }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                categoryrepositery.Edit(category);
                categoryrepositery.Commit();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        public IActionResult Delete(int categoryid)
        {
            Category c = new Category() { Id = categoryid };
            categoryrepositery.Delete(c);
            categoryrepositery.Commit();
            return RedirectToAction("Index");

        }

    }
}
