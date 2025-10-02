using Microsoft.Extensions.DependencyInjection;
using QuotationSystem2.PresentationLayer.Services.Interfaces;
using QuotationSystem2.PresentationLayer.Utilities;
using QuotationSystem2.PresentationLayer.ViewModels;
using System;

namespace QuotationSystem2.PresentationLayer
{
    public class NavigationService : INavigationService
    {
        private readonly MainViewModel _mainViewModel;
        private readonly Dictionary<string, ViewModelBase> _viewModelCache = new();

        public NavigationService(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
        public async Task NavigateTo(string key)
        {
            if (!_viewModelCache.TryGetValue(key, out var vm))
            {
                vm = key switch
                {
                    "Login" => App.AppHost.Services.GetRequiredService<LoginVM>(),
                    "Home" => App.AppHost.Services.GetRequiredService<HomeVM>(),
                    "Account" => App.AppHost.Services.GetRequiredService<AccountVM>(),
                    "PriceTable" => App.AppHost.Services.GetRequiredService<PriceTableVM>(),
                    "SealingPlateQuotation" => App.AppHost.Services.GetRequiredService<SealingPlateQuotationVM>(),
                    "SheerStudQuotation" => App.AppHost.Services.GetRequiredService<SheerStudQuotationVM>(),
                    "WaterMeterQuotation" => App.AppHost.Services.GetRequiredService<WaterMeterQuotationVM>(),
                    _ => throw new ArgumentException($"Unknown navigation key: {key}")
                };

                _viewModelCache[key] = vm;

                // Initialize the ViewModel by calling the InitializeAsync method
                await vm.InitializeAsync();
            }

            _mainViewModel.CurrentView = vm;
        }
    }
}

