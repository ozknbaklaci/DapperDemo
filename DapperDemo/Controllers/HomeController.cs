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
