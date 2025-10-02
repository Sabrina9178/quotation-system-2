using Microsoft.Extensions.DependencyInjection;
using QuotationSystem2.ApplicationLayer.Common;
using QuotationSystem2.ApplicationLayer.DTOs;
using QuotationSystem2.ApplicationLayer.Interfaces;
using QuotationSystem2.PresentationLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuotationSystem2.PresentationLayer
{
    /// <summary>
    /// ChangePasswordWindow.xaml 的互動邏輯
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        private readonly IAppStateService _appStateService;
        private readonly IErrorDialogService _errorDialogService;
        private readonly IEmployeeService _employeeService;
        private readonly ILanguageService _languageService;

        public ChangePasswordWindow(IAppStateService appStateService, IErrorDialogService errorDialogService,
                                    IEmployeeService employeeService, ILanguageService languageService)
        {
            InitializeComponent();

            _appStateService = appStateService;
            _errorDialogService = errorDialogService;
            _employeeService = employeeService;
            _languageService = languageService;

            PasswordConstraintTextBlock.Text = _languageService.GetErrorMessages("PasswordConstraintMessage");
        }

        private async void ChangePasswordButton_Clicked(object sender, RoutedEventArgs e)
        {
            SecureString oldInput = OldPasswordBox.SecurePassword;
            SecureString newInput = NewPasswordBox.SecurePassword;

            string oldPlain = SecureStringService.SecureStringToString(oldInput);
            string currentPassword = SecureStringService.SecureStringToString(_appStateService.User!.Password);

            // Old password inspection
            if (oldPlain != currentPassword)
            {
                await _errorDialogService.ShowMessageAsync("PasswordIncorrectMessage");
                return;
            }

            string newPlain = SecureStringService.SecureStringToString(newInput);

            // Password format inspection
            if (string.IsNullOrWhiteSpace(newPlain) ||
                !Regex.IsMatch(newPlain, @"^[A-Za-z0-9]{1,15}$"))
            {
                await _errorDialogService.ShowMessageAsync("PasswordFormatInvalidMessage");
                return;
            }

            // Edit Password
            _appStateService.User.Password = newInput;
            await _employeeService.EditEmployeeAsync(_appStateService.User);

            await _errorDialogService.ShowMessageAsync("ChangePasswordSuccessMessage");

            this.Close();
        }
    }
}
