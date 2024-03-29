﻿using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DapperDemo.Repository.Dapper
{
    public class CompanyRepositorySp : ICompanyRepository
    {
        private readonly IDbConnection _db;

        public CompanyRepositorySp(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<Company> Find(int id)
        {
            return (await _db.QueryAsync<Company>("usp_GetCompany", new { CompanyId = id }, commandType: CommandType.StoredProcedure)).Single();
        }

        public async Task<List<Company>> GetAll()
        {
            return (await _db.QueryAsync<Company>("usp_GetAllCompany", commandType: CommandType.StoredProcedure)).ToList();
        }

        public async Task<Company> Add(Company company)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", 0, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("@Name", company.Name);
            parameters.Add("@Address", company.Address);
            parameters.Add("@City", company.City);
            parameters.Add("@State", company.State);
            parameters.Add("@PostalCode", company.PostalCode);

            await _db.ExecuteAsync("usp_AddCompany", parameters, commandType: CommandType.StoredProcedure);
            company.CompanyId = parameters.Get<int>("CompanyId");

            return company;
        }

        public async Task<Company> Update(Company company)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", company.CompanyId, DbType.Int32);
            parameters.Add("@Name", company.Name);
            parameters.Add("@Address", company.Address);
            parameters.Add("@City", company.City);
            parameters.Add("@State", company.State);
            parameters.Add("@PostalCode", company.PostalCode);

            await _db.ExecuteAsync("usp_UpdateCompany", parameters, commandType: CommandType.StoredProcedure);

            return company;
        }

        public async Task Remove(int id)
        {
            await _db.ExecuteAsync("usp_RemoveCompany", new { CompanyId = id }, commandType: CommandType.StoredProcedure);
        }
    }
}
