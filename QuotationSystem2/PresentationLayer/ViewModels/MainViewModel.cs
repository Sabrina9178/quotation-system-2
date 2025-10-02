using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QuotationSystem2.ApplicationLayer.Common;
using QuotationSystem2.ApplicationLayer.Interfaces;
using QuotationSystem2.ApplicationLayer.Services;
using QuotationSystem2.PresentationLayer.Services.Interfaces;
using QuotationSystem2.PresentationLayer.Utilities;
using System.ComponentModel;
using System.Windows;

namespace QuotationSystem2.PresentationLayer.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly ILogger<MainViewModel> _logger;

        [ObservableProperty] private ViewModelBase _currentView;

        [ObservableProperty] private Visibility menuVisibility = Visibility.Collapsed;


        public MainViewModel(ILogger<MainViewModel> logger)
        {
            _logger = logger;
            _currentView = App.AppHost.Services.GetRequiredService<LoginVM>();
            _appStateService.PropertyChanged += OnAppStateChanged;

            _logger.LogInformation("Success - Building MainViewModel");
        }

        public override async Task InitializeAsync()
        {
            // This only include async initialization logic.
            await Task.CompletedTask;
        }

        private void OnAppStateChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AppStateService.LoginState))
            {
                MenuVisibility = _appStateService.IsLoggedIn ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        [RelayCommand]
        private void SetLanguage(string cultureCode)
        {
            try
            {
                _languageService.SetLanguage(cultureCode);
                _logger.LogDebug("Start - SetLanguage, with culture code: {CultureCode}", cultureCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail - SetLanguage");
                return;
            }
        }

        
    }
}
