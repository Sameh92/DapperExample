using Dapper;
using DapperWebApplicationExample.Data;
using DapperWebApplicationExample.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DapperWebApplicationExample.Repository
{
    public class CompanyRepositoryDapperSP : ICompanyRepository
    {
        private IDbConnection _db;

        public CompanyRepositoryDapperSP(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("defaultConnection"));
            
        }
        public Company Add(Company company)
        {
            var parameters = new DynamicParameters();            
            parameters.Add("@CompanyId", 0, DbType.Int32,direction:ParameterDirection.Output);
            parameters.Add("@Name", company.Name);
            parameters.Add("@Address", company.Address);
            parameters.Add("@City", company.City);
            parameters.Add("@State", company.State);
            parameters.Add("@PostalCode", company.PostalCode);
            _db.Execute("usp_AddCompany", parameters,commandType: CommandType.StoredProcedure);
            company.CompanyId = parameters.Get<int>("CompanyId");
            return company;

        }

        public Company Find(int id)
        {
            return _db.Query<Company>("usp_GetCompany", new {ComoanyId=id}, commandType: CommandType.StoredProcedure).SingleOrDefault();
        }

        public List<Company> GetAll()
        {
            return _db.Query<Company>("usp_UpdateCompany", commandType: CommandType.StoredProcedure).ToList();
        }

        public void Remove(int id)
        {

             _db.Execute("usp_RemoveCompany", new {CompanyId=id} ,commandType: CommandType.StoredProcedure);

        }

        public Company Update(Company company)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", company.CompanyId, DbType.Int32);
            parameters.Add("@Name", company.Name);
            parameters.Add("@Address", company.Address);
            parameters.Add("@City", company.City);
            parameters.Add("@State", company.State);
            parameters.Add("@PostalCode", company.PostalCode);
            _db.Execute("usp_AddCompany", parameters, commandType: CommandType.StoredProcedure);
         
            return company;
        }
    }
}
