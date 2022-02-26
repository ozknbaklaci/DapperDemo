using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDemo.Data;
using DapperDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace DapperDemo.Repository
{
    public class CompanyRepositoryEf : ICompanyRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CompanyRepositoryEf(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Company> Find(int id)
        {
            return await _dbContext.Companies.FirstOrDefaultAsync(x => x.CompanyId == id);
        }

        public async Task<List<Company>> GetAll()
        {
            return await _dbContext.Companies.ToListAsync();
        }

        public async Task<Company> Add(Company company)
        {
            await _dbContext.Companies.AddAsync(company);
            await _dbContext.SaveChangesAsync();

            return company;
        }

        public async Task<Company> Update(Company company)
        {
            _dbContext.Companies.Update(company);
            await _dbContext.SaveChangesAsync();

            return company;
        }

        public async Task Remove(int id)
        {
            var company = await _dbContext.Companies.FirstOrDefaultAsync(x => x.CompanyId == id);
            if (company != null) _dbContext.Companies.Remove(company);
            await _dbContext.SaveChangesAsync();
        }
    }
}
