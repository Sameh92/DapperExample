using Dapper;
using Dapper.Contrib.Extensions;
using DapperWebApplicationExample.Data;
using DapperWebApplicationExample.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DapperWebApplicationExample.Repository
{
    public class CompanyRepositoryDapperContrib : ICompanyRepository
    {
        private IDbConnection _db;

        public CompanyRepositoryDapperContrib(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("defaultConnection"));
            
        }
        public Company Add(Company company)
        {
            var id = _db.Insert(company);
            company.CompanyId =(int) id;
            return company;
        }

        public Company Find(int id)
        {
          return  _db.Get<Company>(id);
        }

        public List<Company> GetAll()
        {
          return _db.GetAll<Company>().ToList();
        }

        public void Remove(int id)
        {
            _db.Delete(new Company { CompanyId=id}); ;

        }

        public Company Update(Company company)
        {
            _db.Update(company);
            return company;
        }
    }
}
