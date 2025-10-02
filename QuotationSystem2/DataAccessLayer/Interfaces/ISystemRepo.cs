using QuotationSystem2.DataAccessLayer.Models.Contexts;
using QuotationSystem2.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotationSystem2.DataAccessLayer.Interfaces
{
    internal interface ISystemRepo
    {
        Task<Employee?> GetByAccountAsync(string account);
        Task<Employee?> GetByIDAsync(int ID);
        Task<bool> ValidatePasswordAsync(string account, string password);
        Task UpdateEmployeeAsync(Employee updatedEmployee);
        Task CreateEmployeeAsync(Employee newEmployee);
        Task<IReadOnlyList<Employee>> LoadEmployeesAsync();
        Task DeleteEmployeeAsync(int employeeID);
        Task<bool> NoDuplicateAccount(string account);
        Task<List<Role>> GetRolesAsync();

    }
}
