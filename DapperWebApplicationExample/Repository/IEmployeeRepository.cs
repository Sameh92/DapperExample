using DapperWebApplicationExample.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapperWebApplicationExample.Repository
{
    public interface IEmployeeRepository
    {
        Employee Find(int id);
        List<Employee> GetAll();
        Employee Add(Employee employee);
        Employee Update(Employee employee);
        Task<Employee> AddAsync(Employee employee);
        void Remove(int id);
    }
}
