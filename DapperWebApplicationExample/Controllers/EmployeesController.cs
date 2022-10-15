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
    public class EmployeesController : Controller
    {
        private readonly ICompanyRepository _compRepo;
        private readonly IEmployeeRepository _empRepo;
        private readonly IAdvanceRepository _advRepo;
        [BindProperty]
        public Employee Employee { get; set; }
      
        public EmployeesController(ICompanyRepository compRepo, IEmployeeRepository empRepo, IAdvanceRepository advRepo)
        {
            _compRepo = compRepo;
            _empRepo = empRepo;
            _advRepo = advRepo; 
        }

       
        public  IActionResult Index(int companyId = 0)
        {
            //First Medhod which make N+1 Call to database

            //List<Employee> employees = _empRepo.GetAll();
            //foreach (Employee obj in employees)
            //{
            //    obj.Company = _compRepo.Find(obj.CompanyId);
            //}

            //Second Method which make 1 Call to database
            var employees= _advRepo.GetEmployeeWithCompany(companyId);

            return View(employees);
        }

        

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> companyList = _compRepo.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.CompanyId.ToString()
            });
            ViewBag.CompanyList=companyList;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> CreatePost()
        {
            if (ModelState.IsValid)
            {
                // _empRepo.Add(Employee);
              await  _empRepo.AddAsync(Employee);
                return RedirectToAction(nameof(Index));
            }
            return View(Employee);
        }

        
        public  IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee = _empRepo.Find(id.GetValueOrDefault());

            IEnumerable<SelectListItem> companyList = _compRepo.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.CompanyId.ToString()
            });
            ViewBag.CompanyList = companyList;
            if (Employee == null)
            {
                return NotFound();
            }
            return View(Employee);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id)
        {
            if (id != Employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
               
                    _empRepo.Update(Employee);               
                
                
                return RedirectToAction(nameof(Index));
            }
            return View(Employee);
        }

        
        public  IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _empRepo.Remove(id.GetValueOrDefault());
            return RedirectToAction(nameof(Index));

           
        }

       
    }
}
