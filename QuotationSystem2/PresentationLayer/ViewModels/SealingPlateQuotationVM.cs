using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using QuotationSystem2.ApplicationLayer.Common;
using QuotationSystem2.ApplicationLayer.Dtos;
using QuotationSystem2.ApplicationLayer.Services;
using QuotationSystem2.PresentationLayer.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuotationSystem2.PresentationLayer.ViewModels
{
    public partial class SealingPlateQuotationVM : QuoteViewModelBase
    {
        #region properties
        [ObservableProperty] public List<ProductDto> productList = new();
        [ObservableProperty] public ProductDto? selectedProduct = null;

        [ObservableProperty] public List<PipeDiameterDto> pipeDiameterList = new();
        [ObservableProperty] public PipeDiameterDto? selectedPipeDiameter = null;
        [ObservableProperty] public bool isPipeDiameterEnabled = false;

        [ObservableProperty] public List<PipeThicknessDto> pipeThicknessList = new();
        [ObservableProperty] public PipeThicknessDto? selectedPipeThickness = null;
        [ObservableProperty] public bool isPipeThicknessEnabled = false;

        [ObservableProperty] public decimal pipeUnitPrice = new();
        [ObservableProperty] public decimal pipeLength = new();
        [ObservableProperty] public decimal pipePrice = new();

        [ObservableProperty] public List<PipeTopDto> pipeTopList = new();
        [ObservableProperty] public PipeTopDto? selectedPipeTop = null;

        [ObservableProperty] public bool isPipeTopEnabled = false;

        [ObservableProperty] public decimal pipeTopUnitPrice = new();
        [ObservableProperty] public int pipeTopQuantity = new();
        [ObservableProperty] public decimal pipeTopPrice = new();

        [ObservableProperty] public string pipeTopUnitPriceText;
        [ObservableProperty] public Visibility isPipeTopQuantityVisible = Visibility.Hidden;

        [ObservableProperty] public decimal socketUnitPrice = new();
        [ObservableProperty] public decimal weldingUnitPrice = new();
        [ObservableProperty] public int socketQuantity = new();
        [ObservableProperty] public int weldingQuantity = new();
        [ObservableProperty] public decimal socketPrice = new();

        [ObservableProperty] public decimal sealingPlatePrice = new();

        [ObservableProperty] public List<CustomerDto> customerList = new();
        [ObservableProperty] public CustomerDto? selectedCustomer = null;
        [ObservableProperty] public decimal customerDiscount = new();

        [ObservableProperty] public decimal totalPrice = new();
        [ObservableProperty] public decimal totalPriceBeforeDiscount = new();

        [ObservableProperty] public decimal finalPrice = new();
        [ObservableProperty] public string note = string.Empty;

        [ObservableProperty] public List<OperatorDto> operatorList = new();
        [ObservableProperty] public OperatorDto? selectedOperator = null;

        [ObservableProperty] decimal operand = new();
        #endregion

        #region ViewModel Properties
        [ObservableProperty] private string leftButtonText;
        [ObservableProperty] public string rightButtonText;
        [ObservableProperty] public ICommand leftButtonCommand;
        [ObservableProperty] public ICommand rightButtonCommand;

        [ObservableProperty] public ObservableCollection<SealingPlateViewDto> viewList = new();

        public RecordPagedCacheService<SealingPlateViewDto> RecordCache { get; private set; } = null!;

        [ObservableProperty] SealingPlateEditDto editingRecord = new();
        [ObservableProperty] public bool isEditMode = false;
        [ObservableProperty] public string pageCountText = "1/1";

        [ObservableProperty] public Visibility deleteRecordButtonVisibility = Visibility.Collapsed;
        [ObservableProperty] public Visibility editRecordButtonVisibility = Visibility.Collapsed;
        [ObservableProperty] public Visibility exportRecordButtonVisibility = Visibility.Visible;
        [ObservableProperty] public Visibility selectingCheckBoxVisibility = Visibility.Visible;

        private Task? _onQuoteInputChangedTask;
        #endregion  


        public SealingPlateQuotationVM(ILogger<SealingPlateQuotationVM> logger)
        {
            _logger.LogInformation("Success - Building SealingPlateQuotationVM");
            PipeTopUnitPriceText = _languageService.GetUIStrings("UnitPrice");

            // Initialize buttons
            LeftButtonText = _languageService.GetUIStrings("Clear");
            RightButtonText = _languageService.GetUIStrings("Finish");
            LeftButtonCommand = new RelayCommand(async () => await ClearQuotation());
            RightButtonCommand = new RelayCommand(async () => await SaveQuotation());

            if (_appStateService.LoginState == false || _appStateService.User == null)
            {
                return;
            }

            // Set initial visibility based on user role
            if (_appStateService.User.Role.Role == UserRole.Admin)
            {
                // Admin specific logic
                DeleteRecordButtonVisibility = Visibility.Visible;
                EditRecordButtonVisibility = Visibility.Visible;
                SelectingCheckBoxVisibility = Visibility.Visible;
            }
            else if (_appStateService.User.Role.Role == UserRole.Employee)
            {
                // Employee specific logic
            }
        }

        public override async Task InitializeAsync()
        {
            await LoadProductListAsync("SealingPlate", ProductList);
            CustomerList = await LoadCustomerList();
            OperatorList = await LoadOperatorList();
            await LoadRecordCacheAsync();
        }

        public override async Task LoadRecordCacheAsync()
        {
            RecordCache = new RecordPagedCacheService<SealingPlateViewDto>(_quoteQueryService.GetSealingPlateRecordsViewAsync, _quoteQueryService.GetTotalRecordCountAsync<SealingPlateViewDto>);
            var result = await RefreshViewListAsync(RecordCache);
            ViewList = result.viewList;
            PageCountText = result.pageCountText;
        }

        public override void OnLanguageChanged(object? sender, EventArgs e)
        {
            // Refresh ProductList
            ProductList = new List<ProductDto>(ProductList);

            // Refresh SelectedProduct
            var tempProduct = SelectedProduct;
            SelectedProduct = default!;
            SelectedProduct = tempProduct;

            // Refresh PipeTopList
            PipeTopList = new List<PipeTopDto>(PipeTopList);

            // Refresh SelectedPipeTop
            var temp = SelectedPipeTop;
            SelectedPipeTop = default!;
            SelectedPipeTop = temp;

            // Refresh CustomerList
            CustomerList = new List<CustomerDto>(CustomerList);

            // Refresh SelectedCustomer
            var tempCustomer = SelectedCustomer;
            SelectedCustomer = default!;
            SelectedCustomer = tempCustomer;

            // Refresh PipeTopUnitPriceText
            PipeTopUnitPriceText = _languageService.GetUIStrings("UnitPrice");
            LeftButtonText = IsEditMode == false ?
                _languageService.GetUIStrings("Clear") :
                _languageService.GetUIStrings("Cancel") + " " + _languageService.GetUIStrings("Edit");
            RightButtonText = IsEditMode == false ?
                _languageService.GetUIStrings("Finish") :
                _languageService.GetUIStrings("Save") + "" + _languageService.GetUIStrings("Edit");

            // Refresh ViewList
            ViewList = new ObservableCollection<SealingPlateViewDto>(ViewList);

        }

        partial void OnSelectedProductChanged(ProductDto? value)
        {
            _onQuoteInputChangedTask = RunAsync(async () =>
            {
                var result = await SetPipeDiameterList(SelectedProduct);
                IsPipeDiameterEnabled = result.isPipeDiameterEnabled;
                PipeDiameterList = result.pipeDiameterList;
            });
        }

        partial void OnSelectedPipeDiameterChanged(PipeDiameterDto? value)
        {
            _onQuoteInputChangedTask = RunAsync(async () =>
            {
                // PipeThickness
                var result = await SetPipeThicknessList(value, SelectedProduct);
                IsPipeThicknessEnabled = result.isPipeThicknessEnabled;
                PipeThicknessList = result.list;

                // SealingPlatePrice
                SealingPlatePrice = await SetSealingPlatePriceAsync(SelectedPipeDiameter);

                // PipeUnitPrice
                PipeUnitPrice = await SetPipeUnitPriceAsync(SelectedPipeThickness, SelectedPipeDiameter);

                // SocketUnitPrice
                SocketUnitPrice = await SetSocketUnitPriceAsync(SelectedPipeDiameter);

                // WeldingUnitPrice
                WeldingUnitPrice = await SetWeldingUnitPriceAsync(SelectedPipeDiameter);
            });
        }

        private async Task<decimal> SetSocketUnitPriceAsync(PipeDiameterDto? pipeDiameter)
        {
            if (pipeDiameter != null)
            {
                return await _quoteQueryService
                    .GetComponentPriceByNameAndDiameterAsync("SocketSealingPlate", pipeDiameter.DiameterID);
            }
            return 0m;
        }

        private async Task<decimal> SetWeldingUnitPriceAsync(PipeDiameterDto? pipeDiameter)
        {
            if (pipeDiameter != null)
            {
                return await _quoteQueryService
                    .GetComponentPriceByNameAndDiameterAsync("Welding", pipeDiameter.DiameterID);
            }
            return 0m;
        }

        private async Task<decimal> SetSealingPlatePriceAsync(PipeDiameterDto? pipeDiameter)
        {
            if (pipeDiameter != null)
            {
                return await _quoteQueryService
                    .GetComponentPriceByNameAndDiameterAsync("SealingPlate", pipeDiameter.DiameterID);
            }
            return 0m;
        }

        partial void OnSelectedPipeThicknessChanged(PipeThicknessDto? value)
        {
            _onQuoteInputChangedTask = RunAsync(async () =>
            {
                PipeUnitPrice = await SetPipeUnitPriceAsync(SelectedPipeThickness, SelectedPipeDiameter);

            });
        }

        partial void OnPipeUnitPriceChanged(decimal value)
        {
            _onQuoteInputChangedTask = Task.Run(() =>
            {
                PipePrice = SetPipePrice(value, PipeLength, SelectedProduct);
            });
        }

        partial void OnPipeLengthChanged(decimal value)
        {
            _onQuoteInputChangedTask = Task.Run(() =>
            {
                PipePrice = SetPipePrice(PipeUnitPrice, value, SelectedProduct);
            });
        }

        partial void OnSelectedPipeTopChanged(PipeTopDto? value)
        {
            _onQuoteInputChangedTask = RunAsync(async () =>
            {
                var result = await SetPipeTopUnitPrice(value, SelectedPipeDiameter);

                PipeTopUnitPrice = result.pipeTopUnitPrice;
                IsPipeTopQuantityVisible = result.isPipeTopQuantityVisible;
                PipeTopUnitPriceText = result.pipeTopUnitPriceText;
            });
        }

        partial void OnPipeTopUnitPriceChanged(decimal oldValue, decimal newValue)
        {
            _onQuoteInputChangedTask = Task.Run(() =>
            {
                PipeTopQuantity = SetPipeTopQuantity(SelectedPipeTop, PipeTopQuantity);
                PipeTopPrice = SetPipeTopPrice(newValue, PipeTopQuantity, SelectedPipeTop, PipeLength, PipeUnitPrice);

            });

        }

        partial void OnPipeTopQuantityChanged(int value)
        {
            _onQuoteInputChangedTask = Task.Run(() =>
            {
                PipeTopPrice = SetPipeTopPrice(PipeTopUnitPrice, value, SelectedPipeTop, PipeLength, PipeUnitPrice);
            });

        }

        partial void OnSocketUnitPriceChanged(decimal value)
        {
            SetSocketPrice();
        }

        partial void OnWeldingUnitPriceChanged(decimal value)
        {
            if (WeldingQuantity < 1)
            {
                WeldingQuantity = 1;
            }
            
            SetSocketPrice();
        }

        partial void OnSocketQuantityChanged(int value)
        {
            SetSocketPrice();
        }

        partial void OnWeldingQuantityChanged(int value)
        {
            SetSocketPrice();
        }

        public void SetSocketPrice()
        {
            SocketPrice = (SocketUnitPrice * SocketQuantity) + (WeldingUnitPrice * WeldingQuantity);
        }

        #region Change TotalPriceBeforeDiscount
        public void SetTotalPriceBeforeDiscount()
        {
            TotalPriceBeforeDiscount = PipePrice + PipeTopPrice + SocketPrice + SealingPlatePrice;
        }

        partial void OnPipePriceChanged(decimal oldValue, decimal newValue)
        {
            _onQuoteInputChangedTask = RunAsync(async () =>
            {
                SetTotalPriceBeforeDiscount();

                // PipeTop
                var Result = await SetPipeTopAsync(SelectedProduct, SelectedPipeDiameter, newValue);
                PipeTopList = Result.pipeTopList;
                IsPipeTopEnabled = Result.isPipeTopEnabled;
                SelectedPipeTop = Result.selectedPipeTop;
            });
        }

        partial void OnPipeTopPriceChanged(decimal oldValue, decimal newValue)
        {
            _onQuoteInputChangedTask = Task.Run(() =>
            {
                SetTotalPriceBeforeDiscount();
            });
        }

        partial void OnSocketPriceChanged(decimal oldValue, decimal newValue)
        {
            _onQuoteInputChangedTask = Task.Run(() =>
            {
                SetTotalPriceBeforeDiscount();
            });
        }

        partial void OnSealingPlatePriceChanged(decimal oldValue, decimal newValue)
        {
            _onQuoteInputChangedTask = Task.Run(() =>
            {
                SetTotalPriceBeforeDiscount();
            });
        }

        partial void OnSelectedCustomerChanged(CustomerDto? value)
        {
            _onQuoteInputChangedTask = Task.Run(() =>
            {
                CustomerDiscount = SetCustomerDiscount(SelectedCustomer);
            });

        }

        #endregion

        #region Change TotalPrice

        partial void OnTotalPriceBeforeDiscountChanged(decimal value)
        {
            _onQuoteInputChangedTask = Task.Run(() =>
            {
                TotalPrice = SetTotalPrice(value, CustomerDiscount);
            });

        }
        partial void OnCustomerDiscountChanged(decimal value)
        {
            _onQuoteInputChangedTask = Task.Run(() =>
            {
                TotalPrice = SetTotalPrice(TotalPriceBeforeDiscount, value);
            });
        }
        #endregion

        #region Change FinalPrice
        partial void OnTotalPriceChanged(decimal oldValue, decimal newValue)
        {
            _onQuoteInputChangedTask = Task.Run(() =>
            {
                FinalPrice = SetFinalPrice(SelectedOperator, Operand, TotalPrice);
            });

        }

        partial void OnOperandChanged(decimal oldValue, decimal newValue)
        {
            _onQuoteInputChangedTask = Task.Run(() =>
            {
                FinalPrice = SetFinalPrice(SelectedOperator, Operand, TotalPrice);
            });
        }

        partial void OnSelectedOperatorChanged(OperatorDto? value)
        {
            _onQuoteInputChangedTask = Task.Run(() =>
            {
                FinalPrice = SetFinalPrice(SelectedOperator, Operand, TotalPrice);
            });
        }
        #endregion


        [RelayCommand]
        public override async Task ClearQuotation()
        {
            // Clear all properties
            EditingRecord = new SealingPlateEditDto();
            ProductList.Clear();
            SelectedProduct = null;
            PipeDiameterList.Clear();
            SelectedPipeDiameter = null;
            IsPipeDiameterEnabled = false;
            PipeThicknessList.Clear();
            SelectedPipeThickness = null;
            IsPipeThicknessEnabled = false;
            PipeUnitPrice = 0;
            PipeLength = 0;
            PipePrice = 0;
            PipeTopList.Clear();
            SelectedPipeTop = null;
            IsPipeTopEnabled = false;
            PipeTopUnitPrice = 0;
            PipeTopQuantity = 0;
            PipeTopPrice = 0;
            IsPipeTopQuantityVisible = Visibility.Hidden;
            SocketPrice = 0;
            SocketUnitPrice = 0;
            WeldingUnitPrice = 0;
            SocketQuantity = 0;
            WeldingQuantity = 0;
            SocketPrice = 0;
            SealingPlatePrice = 0;
            CustomerList.Clear();
            SelectedCustomer = null;
            CustomerDiscount = 0;
            TotalPriceBeforeDiscount = 0;
            TotalPrice = 0;
            FinalPrice = 0;
            Note = string.Empty;
            OperatorList.Clear();
            Operand = 0;
            SelectedOperator = null;

            await InitializeAsync();
        }

        [RelayCommand]
        public override async Task SaveQuotation()
        {
            if (SelectedProduct == null || SelectedPipeDiameter == null || SelectedPipeThickness == null || SelectedPipeTop == null || PipeLength <= 0 || SelectedPipeTop == null || SelectedCustomer == null)
            {
                await _errorDialogService.ShowMessageAsync("FillInAllRequiredFieldsMessage");
                return;
            }
            if (_appStateService.User == null)
            {
                await _errorDialogService.ShowMessageAsync("LoginRequiredMessage");
                return;
            }

            // Create WaterMeterEditDto
            var quotationDto = new SealingPlateEditDto
            {
                SealingPlateID = IsEditMode == true ? EditingRecord?.SealingPlateID ?? 0 : 0, // depends on is edit mode or create mode
                Date = IsEditMode == true ? EditingRecord?.Date ?? DateTime.Now : DateTime.Now,

                ProductID = SelectedProduct.ProductID,
                PipeDiameterID = SelectedPipeDiameter.DiameterID,
                PipeThicknessID = SelectedPipeThickness.ThicknessID,
                PipeUnitPrice = PipeUnitPrice,
                PipeLength = PipeLength,
                PipePrice = PipePrice,
                PipeTopID = SelectedPipeTop.PipeTopID,
                PipeTopUnitPrice = PipeTopUnitPrice == 0 ? null : PipeTopUnitPrice,
                PipeTopQuantity = PipeTopQuantity == 0 ? null : PipeTopQuantity,
                PipeTopPrice = PipeTopPrice == 0 ? null : PipeTopPrice,
                SocketDiameterID = SelectedPipeDiameter.DiameterID,
                SocketUnitPrice = SocketUnitPrice == 0 ? null : SocketUnitPrice,
                SocketQuantity = SocketQuantity == 0 ? null : SocketQuantity,
                WeldingUnitPrice = WeldingUnitPrice,
                WeldingQuantity = WeldingQuantity,
                SocketPrice = SocketPrice,
                SealingPlatePrice = SealingPlatePrice,
                CustomerID = SelectedCustomer.CustomerID,
                CustomerDiscount = CustomerDiscount,
                TotalPriceBeforeDiscount = TotalPriceBeforeDiscount,
                TotalPrice = TotalPrice,
                Note = Note,
                Operator = (SelectedOperator == null || SelectedOperator.OperatorID == 0) ? null : SelectedOperator.OperatorID,
                Operand = Operand == 0 ? null : Operand,
                FinalPrice = FinalPrice,
                EmployeeID = _appStateService.User.EmployeeID,
            };


            // Save quotation
            if (IsEditMode) // Edit an existing record
            {
                await _quoteWriteService.EditSealingPlateRecordAsync(quotationDto);
                _logger.LogInformation("Record edited successfully.");
            }
            else // Create a new record
            {
                await _quoteWriteService.AddSealingPlateRecordAsync(quotationDto);
                _logger.LogInformation("Record saved successfully.");
            }

            // Initialize the view model
            var result = await RefreshViewListAsync(RecordCache);
            ViewList = result.viewList;
            PageCountText = result.pageCountText;

            await ClearQuotation();
        }

        [RelayCommand]
        public override async Task ExportAllRecords()
        {
            //fix
        }

        [RelayCommand]
        public override async Task ExportSelectedRecords()
        {
            //fix
        }

        [RelayCommand]
        public override async Task NextPage()
        {
            if (RecordCache.CurrentPageNum < RecordCache.TotalPageCount)
            {
                await RecordCache.MoveCurrentPageAsync(RecordCache.CurrentPageNum + 1);
                ViewList = RecordCache.GetPageFromCache(RecordCache.CurrentPageNum);
            }
        }

        [RelayCommand]
        public override async Task PreviousPage()
        {
            if (RecordCache.CurrentPageNum >= 1)
            {
                await RecordCache.MoveCurrentPageAsync(RecordCache.CurrentPageNum - 1);
                ViewList = RecordCache.GetPageFromCache(RecordCache.CurrentPageNum);
            }
        }

        [RelayCommand]
        public override async Task DeleteAllRecords()
        {
            // Confirm deletion
            var MessageBoxResult = MessageBox.Show("確定要刪除所有記錄?", "確認刪除", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (MessageBoxResult == MessageBoxResult.No)
                return;

            await _quoteWriteService.DeleteAllRecordsAsync<SealingPlateEditDto>();

            var result = await RefreshViewListAsync(RecordCache);
            ViewList = result.viewList;
            PageCountText = result.pageCountText;
        }

        [RelayCommand]
        public override async Task DeleteSelectedRecords()
        {
            // Delete selected records
            List<int> selectedRecordIDs = ViewList.Where(u => u.IsSelected).Select(u => u.SealingPlateID).ToList();

            // If no record is selected
            if (selectedRecordIDs.Count == 0)
            {
                MessageBox.Show("請選擇至少一條記錄進行刪除", "刪除錯誤", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Confirm deletion
            var MessageBoxResult = MessageBox.Show("確定要刪除勾選的記錄?", "確認刪除", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (MessageBoxResult == MessageBoxResult.No)
                return;

            await _quoteWriteService.DeleteRecordsByIDAsync<SealingPlateEditDto>(selectedRecordIDs);

            var result = await RefreshViewListAsync(RecordCache);
            ViewList = result.viewList;
            PageCountText = result.pageCountText;
        }

        [RelayCommand]
        public override void SelectThisPage()
        {
            foreach (var record in ViewList)
            {
                record.IsSelected = true;
            }
        }

        [RelayCommand]
        public override void EditSelectedRecord()
        {
            List<int> selectedRecordIDs = ViewList.Where(u => u.IsSelected).Select(u => u.SealingPlateID).ToList();

            // If no record or more than one record is selected
            if (selectedRecordIDs.Count != 1)
            {
                MessageBox.Show("請選擇一條記錄進行編輯", "編輯錯誤", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Confirm erase
            var MessageBoxResult = MessageBox.Show("編輯選項會清除目前的輸入, 確定要進行編輯?", "確認編輯", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (MessageBoxResult == MessageBoxResult.No)
                return;

            IsEditMode = true;
        }

        partial void OnIsEditModeChanged(bool oldValue, bool newValue)
        {
            RunAsync(async () =>
            {
                if (newValue) // Enter EditMode
                {
                    // Get Selected Record ID
                    List<int> selectedRecordIDs = ViewList.Where(u => u.IsSelected).Select(u => u.SealingPlateID).ToList();

                    await ClearQuotation();

                    // Set left and right button text
                    LeftButtonText = _languageService.GetUIStrings("Cancel") + " " + _languageService.GetUIStrings("Edit");
                    RightButtonText = _languageService.GetUIStrings("Save") + "" + _languageService.GetUIStrings("Edit");
                    LeftButtonCommand = new RelayCommand(() => CancelEdit());
                    RightButtonCommand = new RelayCommand(async () => await SaveQuotation());

                    // Collapse Buttons
                    DeleteRecordButtonVisibility = Visibility.Collapsed;
                    ExportRecordButtonVisibility = Visibility.Collapsed;
                    EditRecordButtonVisibility = Visibility.Collapsed;
                    SelectingCheckBoxVisibility = Visibility.Collapsed;

                    // Load selected record from Db
                    EditingRecord = await _quoteQueryService.GetSealingPlateRecordByIdAsync(selectedRecordIDs.First());
                    if (EditingRecord == null)
                    {
                        _logger.LogError("Selected record not found in database.");
                        return;
                    }

                    #region Fill in the fields
                    // Pipe
                    await SetAndWaitAsync(() => SelectedProduct = ProductList.FirstOrDefault(p => p.ProductID == EditingRecord.ProductID));
                    await SetAndWaitAsync(() => SelectedPipeDiameter = PipeDiameterList.FirstOrDefault(d => d.DiameterID == EditingRecord.PipeDiameterID));
                    await SetAndWaitAsync(() => SelectedPipeThickness = PipeThicknessList.FirstOrDefault(t => t.ThicknessID == EditingRecord.PipeThicknessID));
                    await SetAndWaitAsync(() => PipeUnitPrice = EditingRecord.PipeUnitPrice);
                    await SetAndWaitAsync(() => PipeLength = EditingRecord.PipeLength);
                    await SetAndWaitAsync(() => PipePrice = EditingRecord.PipePrice);
                    // PipeTop
                    await SetAndWaitAsync(() => SelectedPipeTop = PipeTopList.FirstOrDefault(t => t.PipeTopID == EditingRecord.PipeTopID));
                    await SetAndWaitAsync(() => PipeTopUnitPrice = EditingRecord.PipeTopUnitPrice ?? 0);
                    await SetAndWaitAsync(() => PipeTopQuantity = EditingRecord.PipeTopQuantity ?? 0);
                    await SetAndWaitAsync(() => PipeTopPrice = EditingRecord.PipeTopPrice ?? 0);
                    // Socket
                    await SetAndWaitAsync(() => SocketUnitPrice = EditingRecord.SocketUnitPrice ?? 0);
                    await SetAndWaitAsync(() => SocketQuantity = EditingRecord.SocketQuantity ?? 0);
                    await SetAndWaitAsync(() => WeldingUnitPrice = EditingRecord.WeldingUnitPrice);
                    await SetAndWaitAsync(() => WeldingQuantity = EditingRecord.WeldingQuantity);
                    // SealingPlate
                    await SetAndWaitAsync(() => SealingPlatePrice = EditingRecord.SealingPlatePrice);
                    // TotalPrice
                    await SetAndWaitAsync(() => SelectedCustomer = CustomerList.Where(c => c.CustomerID == EditingRecord.CustomerID).FirstOrDefault());
                    await SetAndWaitAsync(() => CustomerDiscount = EditingRecord.CustomerDiscount);
                    await SetAndWaitAsync(() => TotalPriceBeforeDiscount = EditingRecord.TotalPriceBeforeDiscount);
                    await SetAndWaitAsync(() => TotalPrice = EditingRecord.TotalPrice);
                    // Note
                    await SetAndWaitAsync(() => Note = EditingRecord.Note ?? "");
                    await SetAndWaitAsync(() => SelectedOperator = OperatorList.Where(o => o.OperatorID == EditingRecord.Operator).FirstOrDefault());
                    await SetAndWaitAsync(() => Operand = EditingRecord.Operand ?? 0);
                    await SetAndWaitAsync(() => FinalPrice = EditingRecord.FinalPrice);
                    #endregion
                }
                else // Quit EditMode
                {
                    await ClearQuotation();

                    // Set left and right button text
                    LeftButtonText = _languageService.GetUIStrings("Clear");
                    RightButtonText = _languageService.GetUIStrings("Finish");
                    LeftButtonCommand = new RelayCommand(async () => await ClearQuotation());
                    RightButtonCommand = new RelayCommand(async () => await SaveQuotation());

                    // Visible Buttons
                    DeleteRecordButtonVisibility = Visibility.Visible;
                    ExportRecordButtonVisibility = Visibility.Visible;
                    EditRecordButtonVisibility = Visibility.Visible;
                    SelectingCheckBoxVisibility = Visibility.Visible;
                }
            });
        }

        public override void CancelEdit()
        {
            // Confirm Cancellation
            var MessageBoxResult = MessageBox.Show("確定要取消編輯?", "取消編輯", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (MessageBoxResult == MessageBoxResult.No)
                return;

            IsEditMode = false;
        }

        public override async Task SetAndWaitAsync(Action setAction)
        {
            // Ensure the action finished running on the UI thread
            setAction();

            if (_onQuoteInputChangedTask != null)
                await _onQuoteInputChangedTask;
        }

    }
}
