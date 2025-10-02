using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using QuotationSystem2.ApplicationLayer.DTOs;
using QuotationSystem2.PresentationLayer.Utilities;
using QuotationSystem2.PresentationLayer.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace QuotationSystem2.PresentationLayer.ViewModels
{
    public partial class AccountVM : ViewModelBase
    {
        private readonly ILogger<AccountVM> _logger;
        [ObservableProperty] private Visibility isVisible = Visibility.Collapsed;
        [ObservableProperty] private ObservableCollection<EmployeeDto> userList = new();
        [ObservableProperty] private EmployeeDto user = null!;

        public AccountVM(ILogger<AccountVM> logger)
        {
            _logger = logger;

            UpdateUserInfo();
            UpdateVisibility();
            _ = SetUserListAsync(); // fire-and-forget
            
            _appStateService.PropertyChanged += OnAppStateChanged;
            _languageService.LanguageChanged += OnLanguageChanged;

            _logger.LogInformation("Success - Building AccountVM");
        }

        public override async Task InitializeAsync()
        {
            await Task.CompletedTask;
        }

        private async void OnLanguageChanged(object? sender, EventArgs e)
        {
            await SetUserListAsync();
            OnPropertyChanged(nameof(User));
        }

        private void OnAppStateChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateUserInfo();
            UpdateVisibility();
        }

        private void UpdateUserInfo()
        {
            User = _appStateService.User ?? null!;

            _logger.LogInformation("Success - UpdateUser");
        }

        private void UpdateVisibility()
        {
            IsVisible = (_appStateService.User?.Role.Role == ApplicationLayer.Common.UserRole.Admin) ? Visibility.Visible : Visibility.Collapsed;
        }

        private async Task SetUserListAsync()
        {
            
            try
            {
                _logger.LogDebug("Start - SetUserList");

                var employeeList = await _employeeService.GetEmployeeListAsync();

                UserList.Clear();
                foreach (var emp in employeeList)
                {
                    UserList.Add(emp);
                }

                _logger.LogInformation("Success - SetUserList, total {Count} items", UserList.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail - SetUserList");
                await _errorDialogService.ShowMessageAsync("SetUserListFailMessage");
            }
        }

        [RelayCommand]
        private async Task DeleteSelectedUsersAsync()
        {
            _logger.LogInformation("Start - DeleteSelectedUsers");

            var selectedUsers = UserList.Where(u => u.IsSelected).ToList();

            // If select oneself
            foreach (var user in selectedUsers)
            {
                if (user.EmployeeID == _appStateService.User?.EmployeeID)
                {
                    _logger.LogWarning("Block - {@Action}", new { Action = "DeleteSelectedUsers", Reason = "CannotDeleteOwnAccount" });
                    await _errorDialogService.ShowMessageAsync("CannotDeleteOneselfMessage");
                    return;
                }
            }

            // If no user is selected
            if (!selectedUsers.Any())
            {
                _logger.LogWarning("Block - {@Action}", new { Action = "DeleteSelectedUsers", Reason = "NoUserSelected" });
                await _errorDialogService.ShowMessageAsync("SelectUserMessage");
                return;
            }

            // Repeat confirmation
            var confirm = MessageBox.Show("確定要刪除所選使用者？", "確認刪除", MessageBoxButton.YesNo);
            if (confirm != MessageBoxResult.Yes)
            {
                _logger.LogInformation("Cancel - DeleteSelectedUsers");
                return;
            }

            // Delete
            var employeeIDs = selectedUsers.Select(u => u.EmployeeID).ToList();
            try
            {
                await _employeeService.DeleteEmployeesAsync(employeeIDs);
                _logger.LogInformation("Success - DeleteSelectedUsers {@Result}", new { Count = employeeIDs.Count, IDs = employeeIDs });

                foreach (var user in selectedUsers)
                {
                    UserList.Remove(user);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail - DeleteSelectedUsers");
                await _errorDialogService.ShowMessageAsync("DeleteErrorMessage");
            }
        }

        [RelayCommand]
        private async Task CreateUserAsync()
        {
            _logger.LogInformation("Start - CreateUser");

            // Create Window
            var dialog = new CreateOrEditUserWindow(createMode: true) { Owner = Application.Current.MainWindow };
            dialog.ShowDialog();

            if (dialog.ViewModel.IsConfirmed)
            {
                var employeeDto = await dialog.ViewModel.ToDto();
                if (employeeDto == null)
                {
                    // Account repeated
                    _logger.LogWarning("Block - {@Action}", new { Action = "CreateUser", Reason = "AccountRepeated" });
                    return;
                }

                try
                {
                    // Create user
                    await _employeeService.CreateEmployeeAsync(employeeDto);
                    _logger.LogInformation("Success - CreateUser {@Result}", new { employeeDto.Account, employeeDto.Name });
                    // Update Userlist
                    await SetUserListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Fail - CreateUser");
                    await _errorDialogService.ShowMessageAsync("CreateUserErrorMessage");
                }
            }
            else
            {
                _logger.LogInformation("Cancel - CreateUser");
            }
        }

        [RelayCommand]
        private async Task EditUserAsync()
        {
            _logger.LogInformation("Start - EditUser");

            var selected = UserList.Where(u => u.IsSelected).ToList();

            // Select one user
            if (selected.Count != 1)
            {
                _logger.LogWarning("Block - {@Action}", new { Action = "EditUser", Reason = "SelectNotExactlyOneUser", SelectedCount = selected.Count });
                await _errorDialogService.ShowMessageAsync("SelectOneUserMessage");
                return;
            }

            // Create Edit Window
            var originalDto = selected.First();
            var dialog = new CreateOrEditUserWindow(createMode: false, dto: originalDto) 
                { 
                    Owner = Application.Current.MainWindow 
                };
            dialog.ShowDialog();

            if (dialog.ViewModel.IsConfirmed)
            {
                var editedDto = await dialog.ViewModel.ToDto();

                // Account repeated
                if (editedDto == null)
                {
                    _logger.LogWarning("Block - {@Action}", new { Action = "EditUser", Reason = "AccountRepeated" });
                    return;
                }

                try
                {
                    await _employeeService.EditEmployeeAsync(editedDto);
                    await SetUserListAsync();

                    _logger.LogInformation("Success - EditUser {@Result}", new { EmployeeID = editedDto.EmployeeID });

                    if (editedDto.EmployeeID == _appStateService?.User?.EmployeeID)
                    {
                        await _appStateService.UpdateUser();
                        // Will trigger OnAppStateChanged
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Fail - EditUser");
                    await _errorDialogService.ShowMessageAsync("EditErrorMessage");
                }
            }
            else
            {
                _logger.LogInformation("Cancel - EditUser");
            }
        }

        [RelayCommand]
        private void ChangePassword()
        {
            _logger.LogInformation("Start - ChangePassword");

            try
            {
                var dialog = new ChangePasswordWindow(languageService: _languageService,
                                                      appStateService: _appStateService,
                                                      errorDialogService: _errorDialogService,
                                                      employeeService: _employeeService)
                {
                    Owner = Application.Current.MainWindow
                };

                dialog.ShowDialog();

                _logger.LogInformation("Success - ChangePassword");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail - ChangePassword");
            }
        }
    }
}
