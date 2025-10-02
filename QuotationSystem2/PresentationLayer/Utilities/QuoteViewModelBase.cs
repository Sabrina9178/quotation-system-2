using QuotationSystem2.ApplicationLayer.Dtos;
using QuotationSystem2.ApplicationLayer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuotationSystem2.PresentationLayer.Utilities
{
    public abstract partial class QuoteViewModelBase : ViewModelBase
    {

        public QuoteViewModelBase() { }

        public override async Task InitializeAsync()
        {
            // Initialization logic for QuoteViewModelBase
            // This can include loading initial data, setting up commands, etc.
            await Task.CompletedTask; // Placeholder for actual initialization logic
        }

        #region Abstract Methods
        public abstract Task LoadRecordCacheAsync();

        public abstract void OnLanguageChanged(object? sender, EventArgs e);

        public abstract Task ClearQuotation();

        public abstract Task SaveQuotation();

        public abstract Task ExportAllRecords();

        public abstract Task ExportSelectedRecords();

        public abstract Task NextPage();

        public abstract Task PreviousPage();

        public abstract Task DeleteAllRecords();

        public abstract Task DeleteSelectedRecords();

        public abstract void SelectThisPage();

        public abstract void EditSelectedRecord();

        public abstract void CancelEdit();

        public abstract Task SetAndWaitAsync(Action setAction);
        #endregion
        public async Task LoadProductListAsync(string productSuffix, List<ProductDto> productList)
        {
            // Load Product List
            var products = await _quoteQueryService.GetProductsBySuffixAsync(productSuffix);
            productList.Clear();
            foreach (var item in products)
            {
                productList.Add(item);
            }
        }

        public async Task<ProductDto> LoadProduct(string productName)
        {
            var products = await _quoteQueryService.GetProductsBySuffixAsync(productName);
            return (products[0]);
        }

        public async Task<List<PipeDiameterDto>> LoadPipeDiametersAsync()
        {
            // Set PipeDiameterList
            var list = await _quoteQueryService.GetDiameters();
            return list.ToList();
        }

        public async Task<List<CustomerDto>> LoadCustomerList()
        {
            // Load Customer List
            var list = await _quoteQueryService.GetCustomersAsync();
            return list.ToList();
        }

        public async Task<List<OperatorDto>> LoadOperatorList()
        {
            // Load Operator List
            var list = await _quoteQueryService.GetOperatorsAsync();
            return list.ToList();
        }

        public ObservableCollection<T> LoadViewListAsync<T>(RecordPagedCacheService<T> recordCache) where T : class
        {
            return recordCache.GetPageFromCache(recordCache.CurrentPageNum);
        }

        public async Task<(string pageCountText, ObservableCollection<T> viewList)> 
            RefreshViewListAsync<T>(RecordPagedCacheService<T> recordCache) where T : class
        {
            await recordCache.ResetCacheAsync();
            var viewList = LoadViewListAsync(recordCache);

            return ($"{recordCache.CurrentPageNum} / {recordCache.TotalPageCount}", viewList);
        }

        public async Task<(bool isPipeDiameterEnabled, List<PipeDiameterDto> pipeDiameterList)> SetPipeDiameterList(ProductDto? SelectedProduct)
        {
            if (SelectedProduct != null)
            {
                var pipeDiameterList = await _quoteQueryService.GetComponentDiametersByNameAsync("SealingPlate");
                return (true, pipeDiameterList);
            }
            else
            {
                return(false, new List<PipeDiameterDto>());
            }
        }


        public async Task<(List<PipeTopDto> pipeTopList, bool isPipeTopEnabled, PipeTopDto? selectedPipeTop)> 
            SetPipeTopAsync(ProductDto? product, PipeDiameterDto? selectedPipeDiameter, decimal pipePrice)
        {
            List<PipeTopDto> pipeTopList = new();
            PipeTopDto? selectedPipeTop = null;
            bool isPipeTopEnabled = false;

            if (selectedPipeDiameter != null && pipePrice != 0 && product != null)
            {
                var pipeTops = await _quoteQueryService
                    .GetPipeTopsByProductAndDiameterAsync(product.ProductID, selectedPipeDiameter.DiameterID);
                pipeTopList.AddRange(pipeTops);
                isPipeTopEnabled = true;
            }

            return (pipeTopList, isPipeTopEnabled, selectedPipeTop);
        }


        public decimal SetPipePrice(decimal pipeUnitPrice, decimal pipeLength, ProductDto? product)
        {
            if (pipeUnitPrice != 0 && pipeLength != 0 && product != null)
            {
                return Math.Round(pipeUnitPrice * pipeLength * product.PipeDiscount, 2);
            }
            else
            {
                return 0;
            }
        }

        public async Task<(bool isPipeThicknessEnabled, List<PipeThicknessDto> list)> SetPipeThicknessList(PipeDiameterDto? pipeDiameter, ProductDto? product)
        {
            if (pipeDiameter != null && product != null)
            {
                var items = await _quoteQueryService.GetThicknessesByDiameterAndProductNameAsync(pipeDiameter.DiameterID, product.ProductName);

                // Return tuple
                return (items.Any(), items.ToList());
            }
            else
            {
                return (false, new List<PipeThicknessDto>());
            }
        }

        public async Task<(decimal pipeTopUnitPrice, Visibility isPipeTopQuantityVisible, string pipeTopUnitPriceText)> 
            SetPipeTopUnitPrice(PipeTopDto? pipeTop, PipeDiameterDto? pipeDiameter)
        {
            if (pipeTop != null && pipeDiameter != null)
            {
                var pipeTopUnitPrice = await _quoteQueryService.GetPipeTopUnitPriceByDiameterAndPipeTopAsync(pipeDiameter.DiameterID, pipeTop.PipeTopID);
                if (pipeTop.PricingMethod.MethodName == "Discount")
                {
                    // Discount
                    return (pipeTopUnitPrice, Visibility.Hidden, _languageService.GetUIStrings("Discount"));
                }
                else 
                {
                    // UnitPrice
                    return (pipeTopUnitPrice, Visibility.Visible, _languageService.GetUIStrings("UnitPrice"));
                }
            }
            else
            {
                return (0, Visibility.Hidden, string.Empty);
            }
        }

        public int SetPipeTopQuantity(PipeTopDto? pipeTop, int pipeTopQuantity)
        {
            if (pipeTop != null)
            {
                if (pipeTop.PricingMethod.MethodName == "Discount")
                {
                    return 0;
                }
                else if (pipeTop.PricingMethod.MethodName == "UnitPrice")
                {
                    if (pipeTopQuantity < 1)
                    {
                        return 1;
                    }
                }
                return 0;
            }
            return 0;
        }

        public decimal SetPipeTopPrice(decimal pipeTopUnitPrice, int pipeTopQuantity, PipeTopDto? pipeTop, decimal pipeLength, decimal pipeUnitPrice)
        {
            if (pipeTop != null)
            {
                return pipeTop.PricingMethod.MethodName switch
                {
                    "Discount" => pipeUnitPrice * pipeLength * pipeTopUnitPrice,
                    "UnitPrice" => Math.Round(pipeTopUnitPrice * pipeTopQuantity),
                    _ => 0m
                };
            }

            return 0m;
        }


        public async Task<(bool isEndPlateEnabled, Visibility isEndPlatePriceVisible, decimal endPlatePrice)> SetEndPlate(PipeDiameterDto? pipeDiameter)
        {
            List<PipeDiameterDto> endPlateDiameters = await _quoteQueryService.GetComponentDiametersByNameAsync("EndPlate");

            if (pipeDiameter != null && endPlateDiameters.Any(d => d.DiameterID == pipeDiameter.DiameterID))
            {
                return (true, Visibility.Visible, 0m);
            }
            else
            {
                return (false, Visibility.Hidden, 0m);
            }
        }


        public async Task<decimal> SetEndPlatePriceAsync(bool isEndPlateChecked, PipeDiameterDto? pipeDiameter)
        {
            if (isEndPlateChecked && pipeDiameter != null)
            {
                // 從 service 計算價格
                return await _quoteQueryService
                    .GetComponentPriceByNameAndDiameterAsync("EndPlate", pipeDiameter.DiameterID);
            }
                return 0m;
        }


        public async Task<decimal> SetPipeUnitPriceAsync(PipeThicknessDto? pipeThickness, PipeDiameterDto? pipeDiameter)
        {
            if (pipeThickness != null && pipeDiameter != null)
            {
                // 從 service 取得單位價格
                return await _quoteQueryService
                    .GetPipeUnitPriceByDiameterAndThicknessAsync(pipeDiameter.DiameterID, pipeThickness.ThicknessID);
            }
            else
            {
                return 0m;
            }
        }


        public decimal SetCustomerDiscount(CustomerDto? selectedCustomer)
        {
            if (selectedCustomer != null)
            {
                return selectedCustomer.Discount;
            }
            else
            {
                return 0;
            }
        }

        public void SetTotalPriceBeforeDiscount(List<decimal> prices, decimal totalPriceBeforeDiscount)
        {
            foreach (var price in prices)
            {
                if (price < 0)
                    throw new ArgumentException("Price cannot be negative.");

                totalPriceBeforeDiscount += price;
            }
        } //Not used yet

        public decimal SetFinalPrice(OperatorDto? selectedOperator, decimal operand, decimal totalPrice)
        {
            if (selectedOperator == null || operand == 0 || totalPrice == 0)
            {
                return totalPrice;
            }
            return _quoteQueryService.GetFinalPriceByOperatorAndOperandAsync(selectedOperator, operand, totalPrice);
        }

        public decimal SetTotalPrice(decimal totalPriceBeforeDiscount, decimal customerDiscount)
        {
            return totalPriceBeforeDiscount * customerDiscount;
        }

        //public void
    }
}
