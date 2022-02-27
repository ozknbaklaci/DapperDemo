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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbConnection _db;

        public EmployeeRepository(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<Employee> Find(int id)
        {
            var queries = "SELECT * FROM Employees WHERE EmployeeId = @EmployeeId";
            return (await _db.QueryAsync<Employee>(queries, new { EmployeeId = id })).Single();
        }

        public async Task<List<Employee>> GetAll()
        {
            var queries = "SELECT * FROM Employees";
            return (await _db.QueryAsync<Employee>(queries)).ToList();
        }

        public async Task<Employee> Add(Employee employee)
        {
            var queries = "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId); SELECT CAST(SCOPE_IDENTITY() as int)";

            var id = (await _db.QueryAsync<int>(queries, employee)).Single();
            employee.EmployeeId = id;

            return employee;
        }

        public async Task<Employee> Update(Employee employee)
        {
            var queries = "UPDATE Employees SET Name = @Name, Title = @Title, Email =  @Email, Phone =  @Phone, CompanyId = @CompanyId WHERE EmployeeId = @EmployeeId";
            await _db.ExecuteAsync(queries, employee);

            return employee;
        }

        public async Task Remove(int id)
        {
            var queries = "DELETE FROM Employees WHERE EmployeeId = @EmployeeId";
            await _db.ExecuteAsync(queries, new { EmployeeId = id });
        }
    }
}
