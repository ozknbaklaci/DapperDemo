using System.Collections.Generic;
using System.Threading.Tasks;
using DapperDemo.Models;

namespace DapperDemo.Repository
{
    public interface IBonusRepository
    {
        Task<List<Employee>> GetEmployeeWithCompany(int id);
        Task<Company> GetCompanyWithEmployees(int companyId);
        Task<List<Company>> GetAllCompanyWithEmployees();
        Task AddTestCompanyWithEmployees(Company company);
        Task AddTestCompanyWithEmployeesWithTransaction(Company company);
        Task RemoveRange(int[] companyId);
        Task<List<Company>> FilterCompanyByName(string name);
    }
}
