using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using QuotationSystem2.ApplicationLayer.Common;
using QuotationSystem2.ApplicationLayer.DTOs;

namespace QuotationSystem2.ApplicationLayer.Interfaces
{
    public interface IAppStateService : INotifyPropertyChanged
    {
        bool IsLoggedIn { get; }
        bool LoginState { get; }
        EmployeeDto? User { get; }

        Task<bool> LoginAsync(string account, SecureString password);
        Task UpdateUser();
        Task LogoutAsync();
    }
}
