using CommunityToolkit.Mvvm.ComponentModel;
using QuotationSystem2.ApplicationLayer.Interfaces;
using QuotationSystem2.ApplicationLayer.Services;
using QuotationSystem2.PresentationLayer.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using Microsoft.Extensions.Logging;
using QuotationSystem2.ApplicationLayer.DTOs;

namespace QuotationSystem2.PresentationLayer.ViewModels
{
    
    public partial class HomeVM : ViewModelBase
    {
        [ObservableProperty] private EmployeeDto user = null!;

        public HomeVM()
        {
            UpdateUser();
            _appStateService.PropertyChanged += OnAppStateChanged;
            _languageService.LanguageChanged += OnLanguageChanged;

            _logger.LogInformation("Success - Building HomeVM");
        }

        public override async Task InitializeAsync()
        {
            // This only include async initialization logic.
            await Task.CompletedTask;
        }

        private void OnLanguageChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(User));
        }

        private void OnAppStateChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateUser();
        }

        private void UpdateUser()
        {
            User = _appStateService.User ?? throw new InvalidOperationException("User is not set in AppStateService");
        }

    }
}
