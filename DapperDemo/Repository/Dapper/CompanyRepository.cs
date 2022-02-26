using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DapperDemo.Repository.Dapper
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IDbConnection _db;

        public CompanyRepository(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<Company> Find(int id)
        {
            var queries = "SELECT * FROM Companies WHERE CompanyId = @CompanyId";
            return (await _db.QueryAsync<Company>(queries, new { CompanyId = id })).Single();
        }

        public async Task<List<Company>> GetAll()
        {
            var queries = "SELECT * FROM Companies";
            return (await _db.QueryAsync<Company>(queries)).ToList();
        }

        public async Task<Company> Add(Company company)
        {
            var queries = "INSERT INTO Companies (Name, Address, City, State, PostalCode) VALUES(@Name, @Address, @City, @State, @PostalCode); SELECT CAST(SCOPE_IDENTITY() as int)";

            var id = (await _db.QueryAsync<int>(queries, company)).Single();
            company.CompanyId = id;

            return company;
        }

        public async Task<Company> Update(Company company)
        {
            var queries = "UPDATE Companies SET Name = @Name, Address =  @Address, City =  @City, State =  @State, PostalCode = @PostalCode WHERE CompanyId = @CompanyId";
            await _db.ExecuteAsync(queries, company);

            return company;
        }

        public async Task Remove(int id)
        {

        }
    }
}
