using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using QuotationSystem2.ApplicationLayer.Common;
using QuotationSystem2.ApplicationLayer.DTOs;
using QuotationSystem2.ApplicationLayer.Interfaces;
using QuotationSystem2.DataAccessLayer.Interfaces;
using System.Security;
using System.Windows;

namespace QuotationSystem2.ApplicationLayer.Services
{
    internal partial class AppStateService : ObservableObject, IAppStateService
    {
        #region Fields or Properties
        private readonly ISystemRepo _systemRepo;
        private readonly ILogger<AppStateService> _logger;

        [ObservableProperty] private bool loginState = false;
        [ObservableProperty] public EmployeeDto? user;

        public bool IsLoggedIn => LoginState;
        #endregion

        public AppStateService(ISystemRepo systemRepo, ILogger<AppStateService> logger)
        {
            _systemRepo = systemRepo;
            _logger = logger;
        }

        public async Task<bool> LoginAsync(string account, SecureString password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(account) || password == null || password.Length == 0)
                {
                    return false; // Invalid credentials
                }

                var entity = await _systemRepo.GetByAccountAsync(account);

                if (entity == null)
                {
;                   return false; // Account not found
                }
                    
                User = EmployeeMapper.ToDto(entity);
                if (!SecureStringService.AreSecureStringEqual(User.Password, password))
                {
                    return false;
                }

                User.LastLogin = DateTime.Now;
                LoginState = true;

                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error during login", ex);
            }
        }

        public async Task UpdateUser()
        {
            try
            {
                if (User == null) return;

                var entity = await _systemRepo.GetByIDAsync(User.EmployeeID);
                if (entity != null)
                {
                    User = EmployeeMapper.ToDto(entity);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error updating user information to Db", ex);
            }
            
        }

        public async Task LogoutAsync()
        {
            try
            {
                // Record the"LastLogin" time before logout
                if (User != null)
                {
                    var entity = EmployeeMapper.ToEntity(User);
                    await _systemRepo.UpdateEmployeeAsync(entity);
                }

                // Clear the user state
                User = null;
                LoginState = false;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error during logout", ex);
            }
        }

        partial void OnLoginStateChanged(bool value)
        {
            if (!value)
            {
                User = null;
            }
        }

    }
}
