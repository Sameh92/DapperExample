﻿using Dapper;
using DapperWebApplicationExample.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DapperWebApplicationExample.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private IDbConnection _db;

        public EmployeeRepository(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("defaultConnection"));

        }
        public Employee Add(Employee employee)
        {
            var sql = "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId);"
                       + "SELECT CAST(SCOPE_IDENTITY() as int); ";
            var id = _db.Query<int>(sql, employee).Single();
            employee.EmployeeId = id;
            return employee;
        }

        public Employee Find(int id)
        {
            var sql = "SELECT * FROM Employees WHERE EmployeeId = @Id";
            return _db.Query<Employee>(sql, new { @Id = id }).Single();
        }

        public List<Employee> GetAll()
        {
            var sql = "SELECT * FROM Employees";
            return _db.Query<Employee>(sql).ToList();
        }

        public void Remove(int id)
        {
            var sql = "DELETE FROM Employees WHERE EmployeeId = @Id";
            _db.Execute(sql, new { id });
        }

        public Employee Update(Employee employee)
        {
            var sql = "UPDATE Employees SET Name = @Name, Title = @Title, Email = @Email, Phone = @Phone, CompanyId = @CompanyId WHERE EmployeeId = @EmployeeId";
            _db.Execute(sql, employee);
            return employee;
        }
        public async Task<Employee> AddAsync(Employee employee)
        {
            var sql = "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId);"
                        + "SELECT CAST(SCOPE_IDENTITY() as int); ";
            var id = await _db.QueryAsync<int>(sql, employee);
            employee.EmployeeId = id.Single();
            return employee;
        }
    }
}
