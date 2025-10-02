using QuotationSystem2.ApplicationLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace QuotationSystem2.ApplicationLayer.Common
{
    public class DtoTranslationConverter : IValueConverter
    {
        public string CurrentLanguageCode { get; set; } = Thread.CurrentThread.CurrentUICulture.Name;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<TranslationDto> translations)
            {
                
                var translation = translations.FirstOrDefault(t => t.LanguageCode == CurrentLanguageCode);
                return translation?.DisplayName ?? "[No Translation]";
            }

            return "[Invalid]";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

}
