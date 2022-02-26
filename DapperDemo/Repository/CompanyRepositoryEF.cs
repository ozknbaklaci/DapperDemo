using System.Collections.Generic;
using System.Linq;
using DapperDemo.Data;
using DapperDemo.Models;

namespace DapperDemo.Repository
{
    public class CompanyRepositoryEF : ICompanyRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CompanyRepositoryEF(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Company Find(int id)
        {
            return _dbContext.Companies.FirstOrDefault(x => x.CompanyId == id);
        }

        public List<Company> GetAll()
        {
            return _dbContext.Companies.ToList();
        }

        public Company Add(Company company)
        {
            _dbContext.Companies.Add(company);
            _dbContext.SaveChanges();

            return company;
        }

        public Company Update(Company company)
        {
            _dbContext.Companies.Update(company);
            _dbContext.SaveChanges();

            return company;
        }

        public void Remove(int id)
        {
            var company = _dbContext.Companies.FirstOrDefault(x => x.CompanyId == id);
            if (company != null) _dbContext.Companies.Remove(company);
            _dbContext.SaveChanges();
        }
    }
}
