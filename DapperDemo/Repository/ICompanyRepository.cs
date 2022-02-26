using System.Collections.Generic;
using System.Threading.Tasks;
using DapperDemo.Models;

namespace DapperDemo.Repository
{
    public interface ICompanyRepository
    {
        Task<Company> Find(int id);
        Task<List<Company>> GetAll();
        Task<Company> Add(Company company);
        Task<Company> Update(Company company);
        Task Remove(int id);
    }
}
