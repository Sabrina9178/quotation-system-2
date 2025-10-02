using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using QuotationSystem2.ApplicationLayer.DTOs;
using QuotationSystem2.ApplicationLayer.Interfaces;
using QuotationSystem2.DataAccessLayer.Interfaces;
using QuotationSystem2.DataAccessLayer.Models.SystemModel2;
using QuotationSystem2.DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuotationSystem2.ApplicationLayer.Services
{
    internal class EmployeeService : ObservableObject, IEmployeeService
    {
        private readonly ISystemRepo _systemRepo;

        public EmployeeService(ISystemRepo systemRepo)
        {
            _systemRepo = systemRepo;
        }

        public async Task CreateEmployeeAsync(EmployeeDto dto)
        {
            try
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto), "EmployeeDto cannot be null");

                var entity = EmployeeMapper.ToEntity(dto);
                await _systemRepo.CreateEmployeeAsync(entity);
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("Error creating employee in Db", ex);
            }
        }

        public async Task<List<EmployeeDto>> GetEmployeeListAsync()
        {
            try
            {
                var employees = await _systemRepo.LoadEmployeesAsync();
                return employees?
                    .Select(EmployeeMapper.ToDto)
                    .ToList() ?? new List<EmployeeDto>();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error loading employee list from Db", ex);
            }
        }

        public async Task EditEmployeeAsync(EmployeeDto dto)
        {
            try
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto), "EmployeeDto cannot be null");

                var entity = EmployeeMapper.ToEntity(dto);
                await _systemRepo.UpdateEmployeeAsync(entity);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error updating employee in Db", ex);
            }
        }

        public async Task DeleteEmployeesAsync(List<int> employeeIDs)
        {
            try
            {
                if (employeeIDs == null || !employeeIDs.Any())
                    throw new ArgumentException("Employee IDs cannot be null or empty", nameof(employeeIDs));

                foreach (int ids in employeeIDs)
                {
                    await _systemRepo.DeleteEmployeeAsync(ids);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error deleting employees from Db", ex);
            }
        }

        public async Task<bool> NoDuplicateAccountAsync(string account)
        {
            if (string.IsNullOrWhiteSpace(account))
                throw new ArgumentException("Account cannot be null or empty", nameof(account));

            return await _systemRepo.NoDuplicateAccount(account);
        }

        public async Task<List<RoleDto>> GetRolesAsync()
        {
            try
            {
                var roles = await _systemRepo.GetRolesAsync();

                var dto = roles?.Select(RoleMapper.ToDto).ToList() ?? new List<RoleDto>();
                return dto;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error loading roles from Db", ex);
            }
        }
    }
}
