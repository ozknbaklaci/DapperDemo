using System;
using System.Collections.Generic;
using DapperDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using DapperDemo.Repository;

namespace DapperDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBonusRepository _bonusRepository;

        public HomeController(ILogger<HomeController> logger, IBonusRepository bonusRepository)
        {
            _logger = logger;
            _bonusRepository = bonusRepository;
        }

        public async Task<IActionResult> Index()
        {
            var companies = await _bonusRepository.GetAllCompanyWithEmployees();
            return View(companies);
        }

        public async Task<IActionResult> AddTestRecords()
        {
            var company = new Company()
            {
                Name = $"Test-{Guid.NewGuid()}",
                Address = "test address",
                City = "test city",
                PostalCode = "test postalCode",
                State = "test state",
                Employees = new List<Employee>
                {
                    new Employee()
                    {
                        Email = "test Email",
                        Name = $"Test Name-{Guid.NewGuid()}",
                        Phone = " test phone",
                        Title = "Test Manager"
                    },
                    new Employee()
                    {
                        Email = "test Email 2",
                        Name = $"Test Name 2-{Guid.NewGuid()}",
                        Phone = " test phone 2",
                        Title = "Test Manager 2"
                    }
                }
            };

            await _bonusRepository.AddTestCompanyWithEmployeesWithTransaction(company);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
