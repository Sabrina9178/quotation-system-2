using Microsoft.Extensions.Logging;
using QuotationSystem2.PresentationLayer.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotationSystem2.PresentationLayer.ViewModels
{
    public partial class PriceTableVM : ViewModelBase
    {
        private readonly ILogger<PriceTableVM> _logger;

        public PriceTableVM(ILogger<PriceTableVM> logger)
        {
            _logger = logger;
            _logger.LogInformation("Success - Building PriceTableVM");
        }

        public override async Task InitializeAsync()
        {
            // This only include async initialization logic.
            await Task.CompletedTask;
        }
    }
}
