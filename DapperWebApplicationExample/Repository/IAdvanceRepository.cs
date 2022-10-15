using DapperWebApplicationExample.Models;
using System.Collections.Generic;

namespace DapperWebApplicationExample.Repository
{
    public interface IAdvanceRepository
    {
        List<Employee> GetEmployeeWithCompany(int companyId);
        Company GetCompanyWithEmployees(int companyId);
        List<Company> GetAllCompanyWithEmployees();
        void AddTestCompanyWithEmployees(Company objComp);
        void RemoveRange(int[] companyId);

        List<Company> FilterCompanyByName(string name);
        void AddTestCompanyWithEmployeesWithTransaction(Company objComp);
    }
}
