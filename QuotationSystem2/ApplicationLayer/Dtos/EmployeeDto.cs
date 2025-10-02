using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using QuotationSystem2.ApplicationLayer.Common;
using QuotationSystem2.ApplicationLayer.Dtos;
using QuotationSystem2.DataAccessLayer.Models;
using QuotationSystem2.PresentationLayer.Services.Interfaces;
using QuotationSystem2.PresentationLayer.Services.Localization;
using QuotationSystem2.PresentationLayer.ViewModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security;

namespace QuotationSystem2.ApplicationLayer.DTOs
{
    public partial class EmployeeDto : ObservableObject
    {
        #region Properties
        public int EmployeeID { get; set; }
        public string Name { get; set; } = null!;
        public string Account { get; set; } = null!;

        [ObservableProperty] private RoleDto role = null!;
        public SecureString Password { get; set; } = null!;
        public DateTime LastLogin { get; set; }
        public bool IsRegistered { get; set; } = true;

        [ObservableProperty] private bool isSelected = false;

        #endregion
    }

    public static class EmployeeMapper
    {
        public static EmployeeDto ToDto(Employee entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return new EmployeeDto
            {
                EmployeeID = entity.EmployeeID,
                Name = entity.Name,
                Account = entity.Account,
                Password = SecureStringService.ToSecureString(entity.Password),
                LastLogin = entity.LastLogin,
                Role = RoleMapper.ToDto(entity.Role),
                IsRegistered = entity.IsRegistered
            };
        }

        public static Employee ToEntity(EmployeeDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            return new Employee
            {
                EmployeeID = dto.EmployeeID,
                Name = dto.Name,
                Account = dto.Account,
                RoleID = (int)dto.Role.RoleID,
                Password = SecureStringService.SecureStringToString(dto.Password),
                LastLogin = dto.LastLogin,
                IsRegistered = dto.IsRegistered
            };
        }

    }

    public class RoleDto
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; } = null!;
        public UserRole Role
        {
            get => (UserRole)RoleID;
            set => RoleID = (int)value;
        }
        public int TranslationID { get; set; }
        public List<TranslationDto> DisplayNames { get; set; } = new();
    }


    public static class RoleMapper
    {
        public static RoleDto ToDto(Role entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            return new RoleDto()
            {
                RoleID = entity.RoleID,
                RoleName = entity.RoleName,
                TranslationID = entity.TranslationID,
                DisplayNames = entity.Translation.Translations
                .Select(t => TranslationMapper.ToDto(t))
                .ToList(),
                Role = (UserRole)entity.RoleID
            };
        }
        public static Role ToEntity(RoleDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            return new Role
            {
                RoleID = dto.RoleID,
                RoleName = dto.RoleName,
                TranslationID = dto.TranslationID,
                Translation = new TranslationGroup
                {
                    Translations = dto.DisplayNames.Select(TranslationMapper.ToEntity).ToList()
                }
            };
        }
    }

    
}
