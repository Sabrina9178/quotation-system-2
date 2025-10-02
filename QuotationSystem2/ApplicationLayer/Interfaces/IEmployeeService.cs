using QuotationSystem2.ApplicationLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace QuotationSystem2.ApplicationLayer.Interfaces
{
    public interface IEmployeeService
    {
        Task CreateEmployeeAsync(EmployeeDto dto);
        Task EditEmployeeAsync(EmployeeDto dto);
        Task DeleteEmployeesAsync(List<int> employeeIDs);
        Task<List<EmployeeDto>> GetEmployeeListAsync();
        Task<bool> NoDuplicateAccountAsync(string account);
        Task<List<RoleDto>> GetRolesAsync();
    }
}
