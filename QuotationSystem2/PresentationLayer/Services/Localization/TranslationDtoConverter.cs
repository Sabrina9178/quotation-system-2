using QuotationSystem2.ApplicationLayer.Dtos;
using QuotationSystem2.PresentationLayer.Services.Interfaces;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Threading;
using System.Windows;

namespace QuotationSystem2.PresentationLayer.Services.Localization
{

    public class TranslationDtoConverter : IValueConverter
    {
        private readonly ILanguageService _languageService = LanguageService.Instance;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            

            var list = value as List<TranslationDto>;

            
            if (list == null) return "[Invalid]";

            var currentLang = _languageService.CurrentCulture.Name; // 例如 "en", "zh"

            return list.FirstOrDefault(x => x.LanguageCode == currentLang)?.DisplayName ?? "[No Translation]";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
