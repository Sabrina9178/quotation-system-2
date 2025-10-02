using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using QuotationSystem2.PresentationLayer.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace QuotationSystem2.PresentationLayer.ViewModels
{
    public partial class LoginVM : ViewModelBase
    {

        [ObservableProperty] private string accountInput = string.Empty;
        public SecureString SecurePassword { private get; set; } = new();

        public LoginVM()
        {
            _logger.LogInformation("Success - Building LoginVM");
        }

        public override async Task InitializeAsync()
        {
            // This only include async initialization logic.
            await Task.CompletedTask;
        }

        [RelayCommand]
        private async Task LoginAsync(object parameter)
        {
            var account = AccountInput;
            var passwordBox = parameter as PasswordBox;
            var securePassword = passwordBox?.SecurePassword ?? new SecureString();

            try
            {
                bool success = await _appStateService.LoginAsync(account, securePassword);

                if (success)
                {
                    _logger.LogInformation("Success - Login {@User}", new { Account = account });
                    await NavigateToView("Home");
                }
                else
                {
                    _logger.LogWarning("Block - Login: invalid credentials for {@Account}", account);
                    await _errorDialogService.ShowMessageAsync("LoginFailMessage");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail - Login");
                await _errorDialogService.ShowMessageAsync("LoginFailMessage");
            }
        }
    }
}
