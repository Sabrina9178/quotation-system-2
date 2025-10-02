using QuotationSystem2.ApplicationLayer.Dtos;
using QuotationSystem2.ApplicationLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuotationSystem2.ApplicationLayer.Services
{
    public class RecordPagedCacheService<TDto> : IRecordPagedCacheService<TDto>
    {
        private readonly Func<int, int, Task<List<TDto>>> LoadPageFunc;
        private readonly Func<Task<int>> GetTotalRecordCountFunc;
        private readonly int PageSize;
        private readonly int PreloadRange;
        private readonly Dictionary<int, ObservableCollection<TDto>> Cache = new();

        private CancellationTokenSource _cts = new();

        public int CurrentPageNum { get; private set; }
        public int TotalRecordCount { get; private set; }
        public int TotalPageCount
        {
            get
            {
                if (PageSize <= 0) return 0; // 避免除以零
                return (TotalRecordCount + PageSize - 1) / PageSize;
            }
        }


        public RecordPagedCacheService(Func<int, int, Task<List<TDto>>> loadPageFunc, 
                                       Func<Task<int>> getTotalRecordCountFunc, 
                                       int startPage = 1, int pageSize = 50, int preloadRange = 3)
        {
            LoadPageFunc = loadPageFunc ?? throw new ArgumentNullException(nameof(loadPageFunc));
            GetTotalRecordCountFunc = getTotalRecordCountFunc ?? throw new ArgumentNullException(nameof(getTotalRecordCountFunc));
            CurrentPageNum = startPage;
            PageSize = pageSize;
            PreloadRange = preloadRange;

            Cache.Clear();
        }

        public async Task InitializeAsync()
        {
            await ResetCacheAsync();
        }

        /// <summary>
        /// Load a specific page from the cache
        public ObservableCollection<TDto> GetPageFromCache(int pageNumber)
        {
            if (Cache.TryGetValue(pageNumber, out var data))
            {
                return data;
            }
            return new ObservableCollection<TDto>(); //fix 
        }

        /// <summary>
        /// Update a new current page and preload pages
        /// </summary>
        /// <param name="newPageNum"></param>
        /// <returns></returns>
        public async Task MoveCurrentPageAsync(int newPageNum)
        {
            CurrentPageNum = newPageNum;

            // Remove pages outside the valid range
            RemovePageOutsideRange();

            // Ensure current page is loaded
            if (!Cache.ContainsKey(CurrentPageNum))
            {
                var data = await LoadPageAsync(CurrentPageNum);
                Cache[CurrentPageNum] = new ObservableCollection<TDto>(data);
            }

            // Load previous and next page in background
            await LoadPreloadPagesAsync();
        }

        /// <summary>
        /// Reset the cache and reload the first page
        /// </summary>
        public async Task ResetCacheAsync()
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();
            Cache.Clear();

            // Get total record count
            TotalRecordCount = await GetTotalRecordCountFunc();

            // Load first page
            CurrentPageNum = 1;
            var firstPageData = await LoadPageAsync(CurrentPageNum);
            Cache[CurrentPageNum] = new ObservableCollection<TDto>(firstPageData);

            // Load preload pages in background
            await LoadPreloadPagesAsync();
        }

        /// <summary>
        /// Remove pages that are outside the preload range from the cache
        private void RemovePageOutsideRange()
        {
            var validPages = Enumerable.Range(CurrentPageNum - PreloadRange, PreloadRange * 2 + 1);
            var toRemove = Cache.Keys.Where(k => !validPages.Contains(k)).ToList();
            foreach (var key in toRemove)
            {
                Cache.Remove(key);
            }
        }

        /// <summary>
        /// Load previous and next pages in background
        /// </summary>
        private async Task LoadPreloadPagesAsync()
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            var tasks = new List<Task>();

            for (int offset = -PreloadRange; offset <= PreloadRange; offset++)
            {
                int page = CurrentPageNum + offset;
                if (page <= 0) continue;

                if (!Cache.ContainsKey(page))
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        try
                        {
                            var data = await LoadPageAsync(page);
                            if (!token.IsCancellationRequested)
                            {
                                Cache[page] = new ObservableCollection<TDto>(data);
                            }
                        }
                        catch { /* 忽略錯誤 */ }
                    }, token));
                }
            }

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Load a specific page from the database
        /// </summary>
        private async Task<List<TDto>> LoadPageAsync(int pageNumber)
        {
            return await LoadPageFunc(pageNumber, PageSize);
        }

        
    }
}
