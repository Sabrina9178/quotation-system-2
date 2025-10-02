using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QuotationSystem2.ApplicationLayer.Common;
using QuotationSystem2.ApplicationLayer.Dtos;
using QuotationSystem2.ApplicationLayer.DTOs;
using QuotationSystem2.DataAccessLayer.Models.SystemModel2;
using QuotationSystem2.PresentationLayer.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using QuotationSystem2.ApplicationLayer.Interfaces;
using QuotationSystem2.PresentationLayer.Services.Interfaces;

namespace QuotationSystem2.PresentationLayer.ViewModels
{
    public partial class CreateOrEditUserVM : ViewModelBase
    {
        #region parameter injection
        [ObservableProperty] private string windowTitle = "";
        [ObservableProperty] private string name = "";
        [ObservableProperty] private string account = "";
        [ObservableProperty] private RoleDto role = null!;
        [ObservableProperty] private Visibility passwordVisibility;
        [ObservableProperty] public List<RoleDto> roles = new();

        public int EmployeeID { get; set; }
        public DateTime LastLogin { get; set; }
        public SecureString Password { get; set; } = null!;
        public string DefaultPasswordString { get; set; } = null!;

        private string DefaultPassword = "12345678";

        public bool IsCreateMode { get; }
        public bool IsConfirmed { get; private set; }
        public Action? RequestCloseAction { get; set; }
        
        #endregion

        public CreateOrEditUserVM(bool createMode, EmployeeDto? dto = null)
        {
            IsCreateMode = createMode;
            _ = InitializeAsync(createMode, dto);
        }

        public override async Task InitializeAsync()
        {
            await Task.CompletedTask;
        } 

        private async Task InitializeAsync(bool createMode, EmployeeDto? dto = null)
        {
            DefaultPasswordString = _languageService.GetErrorMessages("DefaultPasswordMessage") + " " + DefaultPassword;

            _logger.LogInformation("Success - Building CreateOrEditUserVM, IsCreateMode = {@Mode}", new { IsCreateMode });

            Roles = await _employeeService.GetRolesAsync();

            if (IsCreateMode)
            {
                Name = "";
                Account = "";
                Role = Roles[0];
                LastLogin = DateTime.Now;
                Password = SecureStringService.ToSecureString("12345678");

                WindowTitle = _languageService.GetUIStrings("Create") + _languageService.GetUIStrings("User");
                PasswordVisibility = Visibility.Visible;

                _logger.LogInformation("Success - CreateOrEditUserVM, initialize Create mode");
            }
            else
            {
                if (dto == null)
                {
                    throw new ArgumentNullException(nameof(dto), "DTO cannot be null in edit mode.");
                }

                EmployeeID = dto.EmployeeID;
                Name = dto.Name;
                Account = dto.Account;
                Role = dto.Role;
                LastLogin = dto.LastLogin;
                Password = dto.Password;

                WindowTitle = _languageService.GetUIStrings("Edit") + _languageService.GetUIStrings("User");
                PasswordVisibility = Visibility.Collapsed;

                _logger.LogInformation("Success - CreateOrEditUserVM, initialized Edit Mode for Employee {@Employee}", new { dto.EmployeeID });
            }
        }

        public bool Validate() =>
            !string.IsNullOrWhiteSpace(Name) &&
            !string.IsNullOrWhiteSpace(Account);

        async public Task<EmployeeDto?> ToDto(EmployeeDto? source = null)
        {
            _logger.LogInformation("Start - ToDto, IsCreateMode = {@Mode}", new { IsCreateMode });

            if (IsCreateMode == true)
            {
                bool pass = await _employeeService.NoDuplicateAccountAsync(Account);
                if (!pass)
                {
                    _logger.LogWarning("Block - ToDto {@Reason}", new { Reason = "DuplicateAccount", Account });
                    await _errorDialogService.ShowMessageAsync("AccountDuplicatedMessage");
                    return null;
                }
            }

            try
            {
                var dto = source ?? new EmployeeDto
                {
                    Name = Name,
                    Account = Account,
                    Role = Role,
                    Password = Password,
                    LastLogin = LastLogin,
                    IsRegistered = true
                };

                if (!IsCreateMode)
                {
                    dto.EmployeeID = EmployeeID;
                }

                _logger.LogInformation("Success - ToDto {@Result}", new
                {
                    dto.EmployeeID,
                    dto.Account,
                    dto.Role
                });

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail - ToDto");
                return null;
            }
        }

        [RelayCommand]
        private void CancelUserCreation()
        {
            _logger.LogInformation("Success - CancelUserCreation");

            IsConfirmed = false;
            RequestCloseAction?.Invoke();
        }

        [RelayCommand]
        private void ConfirmUserCreation()
        {
            if (!Validate())
            {
                _logger.LogWarning("Block - {@Action}", new { Action = "ConfirmUserCreation", Reason = "ValidationFailed" });
                _errorDialogService.ShowMessageAsync("FillInAllRequiredFieldsMessage");
                return;
            }

            _logger.LogInformation("Success - ConfirmUserCreation");
            IsConfirmed = true;
            RequestCloseAction?.Invoke();
        }

    }

}
