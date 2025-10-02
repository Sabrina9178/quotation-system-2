using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotationSystem2.PresentationLayer.Services.Interfaces
{
    public interface INavigationService
    {
        Task NavigateTo(string pageKey);
    }
}
