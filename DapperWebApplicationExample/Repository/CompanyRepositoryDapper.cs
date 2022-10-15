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
    public class CompanyRepositoryDapper : ICompanyRepository
    {
        private IDbConnection _db;

        public CompanyRepositoryDapper(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("defaultConnection"));
            
        }
        public Company Add(Company company)
        {
            var sql = "INSERT INTO Companies (Name, Address, City, State, PostalCode) VALUES(@Name, @Address, @City, @State, @PostalCode);"
               + "SELECT CAST(SCOPE_IDENTITY() as int); ";

            //First Method

            //var id = _db.Query<int>(sql, new
            //{
            //    @Name = company.Name,
            //    @Address = company.Address,
            //    @City = company.City,
            //    @State = company.State,
            //    @PostalCode = company.PostalCode
            //}).Single();

            //Second Method Since the name of the pramater and name of property  

            //var id = _db.Query<int>(sql, new
            //{
            //    company.Name,
            //    company.Address,
            //    company.City,
            //    company.State,
            //    company.PostalCode
            //}).Single();

            //Third Method

            var id = _db.Query<int>(sql,company).Single();
            company.CompanyId = id;
            return company;

        }

        public Company Find(int id)
        {
            var sql = "SELECT * FROM Companies WHERE CompanyId = @CompanyId";
            return _db.Query<Company>(sql, new { @CompanyId = id }).Single();
        }

        public List<Company> GetAll()
        {
            var sql = "SELECT * FROM Companies";
            return _db.Query<Company>(sql).ToList();
        }

        public void Remove(int id)
        {
            //First Method

            var sql = "DELETE FROM Companies WHERE CompanyId = @Id";
            _db.Execute(sql, new { id });

            //Second Method

            //var sql = "DELETE FROM Companies WHERE CompanyId = @Id";
            //_db.Execute(sql, new { @ID = id });


        }

        public Company Update(Company company)
        {
            //First Method

            var sql = "UPDATE Companies SET Name = @Name, Address = @Address, City = @City, " +
                      "State = @State, PostalCode = @PostalCode WHERE CompanyId = @CompanyId";

            _db.Execute(sql, company);
            //Second Method

            //var sql = "UPDATE Companies SET Name = @Name, Address = @Address, City = @City, " +
            //          "State = @State, PostalCode = @PostalCode WHERE CompanyId = @Id";
            //_db.Execute(sql, new { @Id =company.CompanyId,
            //    @Name = company.Name,
            //    @Address = company.Address,
            //    @City = company.City,
            //    @State = company.State,
            //    @PostalCode = company.PostalCode
            //});
            return company;
        }
    }
}
