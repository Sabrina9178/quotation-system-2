using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotationSystem2.ApplicationLayer.Interfaces
{
    public interface IRecordPagedCacheService<TDto>
    {
        public ObservableCollection<TDto> GetPageFromCache(int pageNumber);

        public Task MoveCurrentPageAsync(int newPage);

        public Task ResetCacheAsync();

        public Task InitializeAsync();
    }
}
