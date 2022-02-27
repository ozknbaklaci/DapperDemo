using System.Collections.Generic;
using System.Threading.Tasks;
using DapperDemo.Models;

namespace DapperDemo.Repository
{
    public interface IEmployeeRepository
    {
        Task<Employee> Find(int id);
        Task<List<Employee>> GetAll();
        Task<Employee> Add(Employee employee);
        Task<Employee> Update(Employee employee);
        Task Remove(int id);
        Task<List<Employee>> GetEmployeeWithCompany();
    }
}
