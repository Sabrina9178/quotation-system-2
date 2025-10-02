using Microsoft.Extensions.DependencyInjection;
using QuotationSystem2.ApplicationLayer.Common;
using QuotationSystem2.PresentationLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotationSystem2.PresentationLayer.Services
{
    internal class ErrorDialogService : IErrorDialogService
    {
        private readonly ILanguageService _languageService;
        public ErrorDialogService(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        public async Task ShowMessageAsync(string code)
        {
            var message = _languageService.GetErrorMessages(code);
            var window = new ErrorWindow(message);
            window.ShowDialog(); // ShowDialog() 是同步方法，不能 await
            await Task.CompletedTask; // 保持 async 方法簽名
        }
    }
}
