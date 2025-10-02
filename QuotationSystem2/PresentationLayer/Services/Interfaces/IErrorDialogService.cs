using QuotationSystem2.ApplicationLayer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotationSystem2.PresentationLayer.Services.Interfaces
{
    public interface IErrorDialogService
    {
        Task ShowMessageAsync(string key);
    }
}
