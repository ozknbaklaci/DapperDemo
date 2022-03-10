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

        public async Task AddTestCompanyWithEmployeesWithTransaction(Company company)
        {
            var queries = "INSERT INTO Companies (Name, Address, City, State, PostalCode) VALUES(@Name, @Address, @City, @State, @PostalCode); SELECT CAST(SCOPE_IDENTITY() as int)";
            var id = (await _db.QueryAsync<int>(queries, company)).Single();
            company.CompanyId = id;

            foreach (var employee in company.Employees)
            {
                employee.CompanyId = company.CompanyId;
                var employeeQueries = "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId); SELECT CAST(SCOPE_IDENTITY() as int)";

                var employeeId = (await _db.QueryAsync<int>(employeeQueries, employee)).Single();
                employee.EmployeeId = employeeId;
            }
        }

        public async Task RemoveRange(int[] companyId)
        {
            await _db.QueryAsync("DELETE FROM Companies WHERE CompanyId IN @companyId", new { companyId });
        }

        public async Task<List<Company>> FilterCompanyByName(string name)
        {
            return (await _db.QueryAsync<Company>("SELECT * FROM Companies WHERE Name like '%' + @name + '%'", new { name })).ToList();
        }
    }
}
