using QuotationSystem2.DataAccessLayer.Interfaces;
using QuotationSystem2.DataAccessLayer.Models.Contexts;
using QuotationSystem2.DataAccessLayer.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace QuotationSystem2.DataAccessLayer.Repositories
{
    internal class SystemRepo : ISystemRepo
    {
        private readonly QuotationSystem2DBContext _context;

        public SystemRepo(QuotationSystem2DBContext DbContext)
        {
            _context = DbContext;
        }

        public async Task<Employee?> GetByAccountAsync(string account)
        {
            return await _context.Employees
                .Include(e => e.Role)
                    .ThenInclude(r => r.Translation)
                    .ThenInclude(t => t.Translations)
                    .ThenInclude(t => t.Language)
                .Where(e => e.IsRegistered == true)
                .FirstOrDefaultAsync(e => e.Account == account);
        }

        public async Task<Employee?> GetByIDAsync(int ID)
        {
            return await _context.Employees
                .Include(e => e.Role)
                .Where(e => e.IsRegistered == true)
                .FirstOrDefaultAsync(e => e.EmployeeID == ID);
        }

        public async Task<bool> ValidatePasswordAsync(string account, string password)
        {
            var employee = await _context.Employees
                .Where(e => e.IsRegistered == true)
                .FirstOrDefaultAsync(e => e.Account == account);
            if (employee == null) return false;

            return employee.Password == password;
            // 實務建議你將來要用密碼雜湊（例如 Hash + Salt）
        }

        public async Task UpdateEmployeeAsync(Employee updatedEmployee)
        {
            var existing = await _context.Employees
                .Where(e => e.IsRegistered == true)
                .FirstOrDefaultAsync(e => e.EmployeeID == updatedEmployee.EmployeeID);

            if (existing == null)
                throw new InvalidOperationException($"找不到 EmployeeID = {updatedEmployee.EmployeeID} 的員工。");

            // 更新欄位
            existing.Name = updatedEmployee.Name;
            existing.Account = updatedEmployee.Account;
            existing.Password = updatedEmployee.Password;
            existing.RoleID = updatedEmployee.RoleID;
            existing.LastLogin = updatedEmployee.LastLogin;

            await _context.SaveChangesAsync();
        }

        public async Task<bool> NoDuplicateAccount(string account)
        {
            var exists = await _context.Employees
                .AnyAsync(e => e.Account == account); // IsRegistered 不需要在這裡檢查，因為這個方法是用來檢查帳號是否存在的，不管它是否已註冊。
            if (exists)
            {
                // throw new InvalidOperationException($"帳號 '{account}' 已存在。");
                return false;
            }
            return true;
        }      

        public async Task CreateEmployeeAsync(Employee newEmployee)
        {
            if (newEmployee == null)
                throw new ArgumentNullException(nameof(newEmployee));

            // DEBUG
            Console.WriteLine($"Creating Employee: {newEmployee.Account}, ID={newEmployee.EmployeeID}");

            await _context.Employees.AddAsync(newEmployee);
            _context.Entry(newEmployee).State = EntityState.Added;

            int result = await _context.SaveChangesAsync();
            Console.WriteLine($"Saved rows: {result}");
        }

        public async Task<IReadOnlyList<Employee>> LoadEmployeesAsync()
        {
            //return await 
            var employees = await
                _context.Employees
                .Include(e => e.Role)
                .ThenInclude(r => r.Translation)
                    .ThenInclude(t => t.Translations)
                        .ThenInclude(t => t.Language)
                .Where(e => e.IsRegistered == true)
                .ToListAsync();

            return employees;
        }

        public async Task DeleteEmployeeAsync(int employeeID)
        {
            var employee = await _context.Employees
                .Where(e => e.IsRegistered == true)
                .FirstOrDefaultAsync(e => e.EmployeeID == employeeID);

            if (employee == null)
                throw new InvalidOperationException($"找不到 EmployeeID = {employeeID} 的員工。");

            if (employee.IsRegistered)
            {
                employee.IsRegistered = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Role>> GetRolesAsync()
        {
            var temp = _context.Roles
                .Include(r => r.Translation)
                    .ThenInclude(r => r.Translations)
                    .ThenInclude(t => t.Language)
                .ToListAsync();
            return await temp;
        }

    }
}
