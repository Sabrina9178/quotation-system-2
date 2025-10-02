using System.Globalization;
using System.Resources;
using System;
using System.Windows;
using QuotationSystem2.ApplicationLayer.Dtos;
using QuotationSystem2.PresentationLayer.Services.Interfaces;

namespace QuotationSystem2.PresentationLayer.Services.Localization
{
    // This is a dual-purpose service  
    // Static instance: is provided for XAML MarkupExtension usage during design time  
    // Implements ILanguageService: Instance interface usage for ViewModel and DI container registration
    public class LanguageService : ILanguageService
    {
        // Static instance
        public static LanguageService Instance { get; } = new LanguageService();

        // Implements ILanguageService
        public event EventHandler LanguageChanged = delegate { };
        private CultureInfo _currentCulture = MapCulture(CultureInfo.CurrentUICulture);

        private static CultureInfo MapCulture(CultureInfo culture)
        {
            return culture.TwoLetterISOLanguageName switch
            {
                "zh" => new CultureInfo("zh-TW"),
                "en" => new CultureInfo("en-US"),
                _ => new CultureInfo("en-US") // fallback
            };
        }

        // Resource Managers
        private readonly ResourceManager _errorMessagesRM =
            new ResourceManager("QuotationSystem2.Resources.Language.ErrorMessages", typeof(LanguageService).Assembly);
        private readonly ResourceManager _UIStringsRM =
            new ResourceManager("QuotationSystem2.Resources.Language.UIStrings", typeof(LanguageService).Assembly);

        public CultureInfo CurrentCulture => _currentCulture;

        // String Accessors
        public string GetUIStrings(string key)
        {
            var result = _UIStringsRM.GetString(key, _currentCulture);
            if (result == null)
                throw new KeyNotFoundException($"UI string key not found: {key}");
            return result;
        }

        public string GetErrorMessages(string key)
        {
            var result = _errorMessagesRM.GetString(key, _currentCulture);
            if (result == null)
                throw new KeyNotFoundException($"Error message key not found: {key}");
            return result;
        }


        // Language Setting Method
        public void SetLanguage(string cultureCode)
        {
            try
            {
                _currentCulture = new CultureInfo(cultureCode);
                Thread.CurrentThread.CurrentUICulture = _currentCulture;
                Thread.CurrentThread.CurrentCulture = _currentCulture;

                LanguageChanged?.Invoke(null, EventArgs.Empty);
            }
            catch (CultureNotFoundException ex)
            {
                throw new ArgumentException($"Invalid culture code: {cultureCode}", ex);
            }
        }




        // Get string from translation list
        public string GetTranslation(List<TranslationDto> translationDtos)
        {
            // Check if translationDto is null
            if (translationDtos == null)
            {
                throw new ArgumentNullException(nameof(translationDtos), "Translation DTO cannot be null.");
            }

            // Find the translation for the specified language code
            foreach (var translation in translationDtos)
            {
                if (translation.LanguageCode.Equals(_currentCulture.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return translation.DisplayName;
                }
            }

            return string.Empty;
        }
    }
}
