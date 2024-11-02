using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Repositery.InterfaceCategory;
using E_Commerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    [Authorize(Roles = $"{Sw.adminRole}  ")]

    public class CompanyController : Controller
    {
        private readonly ICompanyRepository companyRepository;
        public CompanyController(ICompanyRepository companyRepository)
        {
            this.companyRepository = companyRepository;
        }

        public IActionResult Index()
        {
                var c =      companyRepository.GetAll([e => e.products]); 
             return View(model: c);
        }
        public IActionResult Create()
        {
            Company company = new Company();
            return View(company);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                companyRepository.Add(company);
                companyRepository.Commit(); 
                 return RedirectToAction("Index");
            }
            return View(company);
        }
        public IActionResult Edit(int companyid) {
            var cat = companyRepository.GetOne([], e => e.Id == companyid); 

             return View(model: cat);
        }
        [HttpPost]
        public IActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                companyRepository.Edit(company);
                companyRepository.Commit(); 
 
                return RedirectToAction("Index");
            }
            return View(company);
        }

    }
}
