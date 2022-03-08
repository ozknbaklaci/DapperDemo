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
    public class BonusRepository : IBonusRepository
    {
        private readonly IDbConnection _db;

        public BonusRepository(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        //One to One Relation in Dapper
        public async Task<List<Employee>> GetEmployeeWithCompany(int id)
        {
            var queries =
                "SELECT em.*, co.* FROM Employees as em INNER JOIN Companies as co ON em.CompanyId = co.CompanyId";
            //with Parameters
            if (id != 0)
            {
                queries += " WHERE em.CompanyId = @Id";
            }
            var employee = await _db.QueryAsync<Employee, Company, Employee>(queries, (e, c) =>
             {
                 e.Company = c;
                 return e;
             }, new { Id = id }, splitOn: "CompanyId");

            return employee.ToList();
        }

        public async Task<Company> GetCompanyWithEmployees(int companyId)
        {
            var parameter = new
            {
                CompanyId = companyId
            };

            var queries =
                "SELECT * FROM Companies WHERE CompanyId = @CompanyId; SELECT * FROM Employees WHERE CompanyId = @CompanyId;";

            using var results = await _db.QueryMultipleAsync(queries, parameter);
            var company = (await results.ReadAsync<Company>()).ToList().FirstOrDefault();
            company.Employees = (await results.ReadAsync<Employee>()).ToList();

            return company;
        }

        public async Task<List<Company>> GetAllCompanyWithEmployees()
        {
            var queries =
                "SELECT co.*, em.* FROM Employees as em INNER JOIN Companies as co ON em.CompanyId = co.CompanyId";
            var companyDictionary = new Dictionary<int, Company>();

            var company = await _db.QueryAsync<Company, Employee, Company>(queries, (c, e) =>
            {
                if (!companyDictionary.TryGetValue(c.CompanyId, out var currentCompany))
                {
                    currentCompany = c;
                    companyDictionary.Add(currentCompany.CompanyId, currentCompany);
                }
                currentCompany.Employees.Add(e);
                return currentCompany;
            }, splitOn: "EmployeeId");

            return company.Distinct().ToList();
        }
    }
}
