using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DapperDemo.Repository.Dapper
{
    public class CompanyRepositoryContrib : ICompanyRepository
    {
        private readonly IDbConnection _db;

        public CompanyRepositoryContrib(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<Company> Find(int id)
        {
            return await _db.GetAsync<Company>(id);
        }

        public async Task<List<Company>> GetAll()
        {
            return (await _db.GetAllAsync<Company>()).ToList();
        }

        public async Task<Company> Add(Company company)
        {
            var id = await _db.InsertAsync(company);
            company.CompanyId = id;

            return company;
        }

        public async Task<Company> Update(Company company)
        {
            await _db.UpdateAsync(company);

            return company;
        }

        public async Task Remove(int id)
        {
            await _db.DeleteAsync(new Company { CompanyId = id });
        }
    }
}
