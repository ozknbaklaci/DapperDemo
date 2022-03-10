using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DapperDemo.Models;
using DapperDemo.Repository;

namespace DapperDemo.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IBonusRepository _bonusRepository;
        private readonly IDapperStoredProcRepo _dapperStoredProcRepo;

        public CompaniesController(ICompanyRepository companyRepository,
            IEmployeeRepository employeeRepository,
            IBonusRepository bonusRepository,
            IDapperStoredProcRepo dapperStoredProcRepo)
        {
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
            _bonusRepository = bonusRepository;
            _dapperStoredProcRepo = dapperStoredProcRepo;
        }

        // GET: Companies
        public async Task<IActionResult> Index()
        {
            //return View(await _companyRepository.GetAll());

            //Generic Repo Example
            return View(_dapperStoredProcRepo.List<Company>("usp_GetAllCompany"));
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _bonusRepository.GetCompanyWithEmployees(id.GetValueOrDefault());
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
        public async Task<IActionResult> Create([Bind("CompanyId,Name,Address,City,State,PostalCode")] Company company)
        {
            if (ModelState.IsValid)
            {
                await _companyRepository.Add(company);
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var company = await _companyRepository.Find(id.GetValueOrDefault());

            //Generic Repo Example
            var company = _dapperStoredProcRepo.Single<Company>("usp_GetCompany", new { CompanyId = id.GetValueOrDefault() });
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
        public async Task<IActionResult> Edit(int id, [Bind("CompanyId,Name,Address,City,State,PostalCode")] Company company)
        {
            if (id != company.CompanyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _companyRepository.Update(company);

                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _companyRepository.Remove(id.GetValueOrDefault());


            return RedirectToAction(nameof(Index));
        }
    }
}
