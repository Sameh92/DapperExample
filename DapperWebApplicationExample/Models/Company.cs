using Dapper.Contrib.Extensions;
using System.Collections.Generic;


namespace DapperWebApplicationExample.Models
{
    [Table("Companies")]//from dapper contrib not from data anntotation
    public class Company
    {
        [Key]//from dapper contrib not from data anntotation
        public int CompanyId { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
       
        public string PostalCode { get; set; }
        [Write(false)]//from dapper contrib equal to[NotMapped] in data anntotation
        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
