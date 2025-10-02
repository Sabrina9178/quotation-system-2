using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuotationSystem2.ApplicationLayer.Dtos;

namespace QuotationSystem2.PresentationLayer.Services.Interfaces
{
    public interface ILanguageService
    {
        event EventHandler LanguageChanged;

        string GetUIStrings(string key);

        string GetErrorMessages(string key);

        void SetLanguage(string cultureCode);

        string GetTranslation(List<TranslationDto> translationDtos);

        CultureInfo CurrentCulture { get; }
    }
}