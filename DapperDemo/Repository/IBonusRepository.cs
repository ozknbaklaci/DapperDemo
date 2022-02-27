using System.Collections.Generic;
using System.Threading.Tasks;
using DapperDemo.Models;

namespace DapperDemo.Repository
{
    public interface IBonusRepository
    {
        Task<List<Employee>> GetEmployeeWithCompany(int id);
        Task<Company> GetCompanyWithAddresses(int companyId);
    }
}
