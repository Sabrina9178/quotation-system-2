using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QuotationSystem2.ApplicationLayer.Common;
using QuotationSystem2.ApplicationLayer.Interfaces;
using QuotationSystem2.PresentationLayer.Services.Interfaces;

namespace QuotationSystem2.PresentationLayer.Utilities
{
    public abstract partial class ViewModelBase : ObservableObject
    {
        internal INavigationService _navigationService => App.AppHost.Services.GetRequiredService<INavigationService>();
        internal IErrorDialogService _errorDialogService => App.AppHost.Services.GetRequiredService<IErrorDialogService>();
        internal ILanguageService _languageService => App.AppHost.Services.GetRequiredService<ILanguageService>();
        internal IAppStateService _appStateService => App.AppHost.Services.GetRequiredService<IAppStateService>();
        internal IEmployeeService _employeeService => App.AppHost.Services.GetRequiredService<IEmployeeService>();
        internal IQuoteQueryService _quoteQueryService => App.AppHost.Services.GetRequiredService<IQuoteQueryService>();
        internal IQuoteWriteService _quoteWriteService => App.AppHost.Services.GetRequiredService<IQuoteWriteService>();
        internal ILogger<ViewModelBase> _logger => App.AppHost.Services.GetRequiredService<ILogger<ViewModelBase>>();

        public ViewModelBase()
        {
        }

        public abstract Task InitializeAsync();


        [RelayCommand]
        public async Task NavigateToView(string viewName)
        {
            try
            {
                await _navigationService.NavigateTo(viewName);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Invalid navigation key, failed to navigate to '{viewName}' view ", ex);

            }
        }

        public async Task Logout()
        {
            // Navigate to LoginView
            await _navigationService.NavigateTo("LoginView");

            // Clear the app state
            await _appStateService.LogoutAsync();
        }

        public Task RunAsync(Func<Task> asyncAction)
        {
            return Task.Run(async () =>
            {
                try
                {
                    await asyncAction();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Fail.");
                }
            });
        }

        
    }
}
