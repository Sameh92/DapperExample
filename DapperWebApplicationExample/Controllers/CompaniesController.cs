using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DapperWebApplicationExample.Data;
using DapperWebApplicationExample.Models;
using DapperWebApplicationExample.Repository;

namespace DapperWebApplicationExample.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ICompanyRepository _compRepo;
        private readonly IEmployeeRepository _empRepo;
        private readonly IAdvanceRepository _advRepo;
        private readonly IDapperSprocRepoGeneric _genericRepo;
        public CompaniesController(ICompanyRepository compRepo, IEmployeeRepository empRepo,
            IAdvanceRepository advRepo, IDapperSprocRepoGeneric genericRepo)
        {
            _compRepo = compRepo;
            _empRepo = empRepo;
            _advRepo = advRepo;
            _genericRepo = genericRepo;   
        }

        // GET: Companies
        public  IActionResult Index()
        {
            var CompanyList = _genericRepo.ListByProcedureName<Company>("usp_GetALLCompany");
            return View(CompanyList);
            //return View(_compRepo.GetAll());
        }

        // GET: Companies/Details/5
        public  IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // var company = _compRepo.Find(id.GetValueOrDefault());
            var company = _advRepo.GetCompanyWithEmployees(id.GetValueOrDefault());
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Create([Bind("CompanyId,Name,Address,City,State,PostalCode")] Company company)
        {
            if (ModelState.IsValid)
            {
                _compRepo.Add(company);
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Edit/5
        public  IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var company = _genericRepo.SingleByProcedureName<Company>("usp_GetCompany", new { CompanyId = id.GetValueOrDefault() });
            //var company = _compRepo.Find(id.GetValueOrDefault());
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("CompanyId,Name,Address,City,State,PostalCode")] Company company)
        {
            if (id != company.CompanyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
               
                    _compRepo.Update(company);               
                
                
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        
        public  IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _compRepo.Remove(id.GetValueOrDefault());
            return RedirectToAction(nameof(Index));

           
        }

       
    }
}
