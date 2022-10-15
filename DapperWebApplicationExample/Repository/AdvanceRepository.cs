using Dapper;
using DapperWebApplicationExample.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;

namespace DapperWebApplicationExample.Repository
{
    public class AdvanceRepository : IAdvanceRepository
    {
        private IDbConnection _db;
        public AdvanceRepository(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("defaultConnection"));


        }



        public List<Employee> GetEmployeeWithCompany(int companyId)
        {
            var sql = "SELECT E.*,C.* FROM Employees AS E INNER JOIN Companies AS C ON E.CompanyId = C.CompanyId ";
            if (companyId != 0)
            {
                sql += " WHERE E.CompanyId = @Id";
            }
            var employees = _db.Query<Employee, Company, Employee>(sql, (e, c) =>
            {
                e.Company = c;
                return e;
            }, new { @Id = companyId }, splitOn: "CompanyId");

            return employees.ToList();

        }
        public Company GetCompanyWithEmployees(int companyId)
        {
            var p = new
            {
                CompanyId = companyId
            };

            var sql = "SELECT * FROM Companies WHERE CompanyId = @CompanyId;"
                    + " SELECT * FROM Employees WHERE CompanyId = @CompanyId; ";
            Company company;
            using (var lists = _db.QueryMultiple(sql, p))
            {
                company = lists.Read<Company>().ToList().FirstOrDefault();
                company.Employees = lists.Read<Employee>().ToList();

            }
            return company;
        }

        public List<Company> GetAllCompanyWithEmployees()
        {
            var sql = "SELECT C.*,E.* FROM Employees AS E INNER JOIN Companies AS C ON E.CompanyId = C.CompanyId ";
            var companyDic = new Dictionary<int, Company>();
            var company = _db.Query<Company, Employee, Company>(sql, (c, e) =>
                {
                    if (!companyDic.TryGetValue(c.CompanyId, out var cureentCompany))
                    {
                        cureentCompany = c;
                        companyDic.Add(cureentCompany.CompanyId, c);
                    }
                    cureentCompany.Employees.Add(e);
                    return cureentCompany;
                }, splitOn: "EmployeeId");
            return company.Distinct().ToList();

        }

        public void AddTestCompanyWithEmployees(Company objComp)
        {
            var sql = "INSERT INTO Companies (Name, Address, City, State, PostalCode) VALUES(@Name, @Address, @City, @State, @PostalCode);"
                    + "SELECT CAST(SCOPE_IDENTITY() as int); ";
            var id = _db.Query<int>(sql, objComp).Single();
            objComp.CompanyId = id;
            //Normal Insert

            //foreach (var employee in objComp.Employees)
            //{
            //    employee.CompanyId = objComp.CompanyId;
            //    var sql1 = "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId);"
            //           + "SELECT CAST(SCOPE_IDENTITY() as int); ";
            //    _db.Query<int>(sql1, employee).Single();
            //}

            //Bulk Insert

            objComp.Employees.Select(c => { c.CompanyId = id; return c; }).ToList();
            var sqlEmp = "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId);"
                       + "SELECT CAST(SCOPE_IDENTITY() as int); ";
            _db.Execute(sqlEmp, objComp.Employees);
        }
        public void RemoveRange(int[] companyId)
        {
            _db.Query("DELETE FROM Companies WHERE CompanyId IN @companyId", new { companyId });
        }

        public List<Company> FilterCompanyByName(string name)
        {
            return _db.Query<Company>("SELECT * FROM Companies WHERE Name like '%' + @name + '%' ", new { name }).ToList();
        }
        public void AddTestCompanyWithEmployeesWithTransaction(Company objComp)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    var sql = "INSERT INTO Companies (Name, Address, City, State, PostalCode) VALUES(@Name, @Address, @City, @State, @PostalCode);"
                 + "SELECT CAST(SCOPE_IDENTITY() as int); ";
                    var id = _db.Query<int>(sql, objComp).Single();
                    objComp.CompanyId = id;

                    objComp.Employees.Select(c => { c.CompanyId = id; return c; }).ToList();
                    var sqlEmp = "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId);"
                             + "SELECT CAST(SCOPE_IDENTITY() as int); ";
                    _db.Execute(sqlEmp, objComp.Employees);

                    transaction.Complete();
                }
                catch
                {

                }
            }
        }
    }
}
