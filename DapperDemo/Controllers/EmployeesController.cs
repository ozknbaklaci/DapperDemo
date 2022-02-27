﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DapperDemo.Models;
using DapperDemo.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DapperDemo.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeesController(IEmployeeRepository employeeRepository,
            ICompanyRepository companyRepository)
        {
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var employees = await _employeeRepository.GetEmployeeWithCompany();

            return View(employees);
        }

        // GET: Employees/Create
        public async Task<IActionResult> Create()
        {
            var companyList = (await _companyRepository.GetAll()).Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.CompanyId.ToString()
            });

            ViewBag.CompanyList = companyList;

            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _employeeRepository.Add(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeeRepository.Find(id.GetValueOrDefault());
            if (employee == null)
            {
                return NotFound();
            }

            var companyList = (await _companyRepository.GetAll()).Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.CompanyId.ToString()
            });

            ViewBag.CompanyList = companyList;

            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _employeeRepository.Update(employee);

                return RedirectToAction(nameof(Index));
            }

            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _employeeRepository.Remove(id.GetValueOrDefault());

            return RedirectToAction(nameof(Index));
        }
    }
}
