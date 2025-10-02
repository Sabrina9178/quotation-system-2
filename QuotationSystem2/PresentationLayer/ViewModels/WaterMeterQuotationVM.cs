using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using QuotationSystem2.ApplicationLayer.Common;
using QuotationSystem2.ApplicationLayer.Dtos;
using QuotationSystem2.ApplicationLayer.Services;
using QuotationSystem2.PresentationLayer.Utilities;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace QuotationSystem2.PresentationLayer.ViewModels
{
    public partial class WaterMeterQuotationVM : QuoteViewModelBase
    {
        #region Quotation Properties
        [ObservableProperty] public ProductDto product = new();

        [ObservableProperty] public List<PipeDiameterDto> pipeDiameterList = new();
        [ObservableProperty] public PipeDiameterDto? selectedPipeDiameter = null;

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

        [ObservableProperty] public GridLength socketRowHeight = new(60);
        [ObservableProperty] public GridLength nippleRowHeight = new(60);
        [ObservableProperty] public List<PipeDiameterDto> socketDiameterList = new();
        [ObservableProperty] public PipeDiameterDto selectedSocketDiameter = new();

        [ObservableProperty] public ObservableCollection<TeethInputVM> socketInputs = new();
        [ObservableProperty] public ObservableCollection<TeethInputVM> nippleInputs = new();

        [ObservableProperty] public bool isEndPlateEnabled = false;
        [ObservableProperty] public bool isEndPlateChecked = false;
        [ObservableProperty] public decimal endPlatePrice = new();
        [ObservableProperty] public Visibility isEndPlatePriceVisible = Visibility.Hidden;

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

        [ObservableProperty] public ObservableCollection<WaterMeterViewDto> viewList = new();

        public RecordPagedCacheService<WaterMeterViewDto> RecordCache { get; private set; } = null!;

        [ObservableProperty] WaterMeterEditDto editingRecord = new();
        [ObservableProperty] public bool isEditMode = false;
        [ObservableProperty] public string pageCountText = "1/1";

        [ObservableProperty] public Visibility deleteRecordButtonVisibility = Visibility.Collapsed;
        [ObservableProperty] public Visibility editRecordButtonVisibility = Visibility.Collapsed;
        [ObservableProperty] public Visibility exportRecordButtonVisibility = Visibility.Visible;
        [ObservableProperty] public Visibility selectingCheckBoxVisibility = Visibility.Visible;

        private Task? _onQuoteInputChangedTask;
        #endregion  

        public WaterMeterQuotationVM()
        {
            _logger.LogInformation("Success - Building WaterMeterQuotationVM");
            PipeTopUnitPriceText = _languageService.GetUIStrings("UnitPrice");

            // Initialize buttons
            LeftButtonText = _languageService.GetUIStrings("Clear");
            RightButtonText = _languageService.GetUIStrings("Finish");
            leftButtonCommand = new RelayCommand(async () => await ClearQuotation());
            rightButtonCommand = new RelayCommand(async () => await SaveQuotation());

            
            if(_appStateService.LoginState == false || _appStateService.User == null)
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
            Product = await LoadProduct("WaterMeter");
            PipeDiameterList = await LoadPipeDiametersAsync();
            CustomerList = await LoadCustomerList();
            OperatorList = await LoadOperatorList();
            await LoadRecordCacheAsync();
            await AddSocket();
            await AddNipple();

            _languageService.LanguageChanged += OnLanguageChanged;
        }

        public override void OnLanguageChanged(object? sender, EventArgs e)
        {
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
            ViewList = new ObservableCollection<WaterMeterViewDto>(ViewList);


        }

        public override async Task LoadRecordCacheAsync()
        {
            RecordCache = new RecordPagedCacheService<WaterMeterViewDto>(_quoteQueryService.GetWaterMeterRecordsViewAsync, _quoteQueryService.GetTotalRecordCountAsync<WaterMeterViewDto>);
            var result = await RefreshViewListAsync(RecordCache);
            ViewList = result.viewList;
            PageCountText = result.pageCountText;
        }

        partial void OnSelectedPipeDiameterChanged(PipeDiameterDto? value)
        {
            _onQuoteInputChangedTask = RunAsync(async () =>
            {
                // PipeThickness
                var result = await SetPipeThicknessList(value, Product);
                IsPipeThicknessEnabled = result.isPipeThicknessEnabled;
                PipeThicknessList = result.list;

                // EndPlate
                var result2 = await SetEndPlate(value);
                IsEndPlateEnabled = result2.isEndPlateEnabled;
                IsEndPlatePriceVisible = result2.isEndPlatePriceVisible;
                EndPlatePrice = result2.endPlatePrice;

            });
        }

        partial void OnIsEndPlateCheckedChanged(bool oldValue, bool newValue)
        {
            _onQuoteInputChangedTask = RunAsync(async () =>
            {
                EndPlatePrice = await SetEndPlatePriceAsync(newValue, SelectedPipeDiameter);
            });
        }

        partial void OnSelectedPipeThicknessChanged(PipeThicknessDto? value)
        {
            _onQuoteInputChangedTask = RunAsync(async () =>
            {
                PipeUnitPrice = await SetPipeUnitPriceAsync(value, SelectedPipeDiameter);
            });
        }

        partial void OnPipeUnitPriceChanged(decimal value)
        {
            _onQuoteInputChangedTask = Task.Run(() =>
            {
                PipePrice = SetPipePrice(value, PipeLength, Product);
            });
        }

        partial void OnPipeLengthChanged(decimal value)
        {
            _onQuoteInputChangedTask = Task.Run(() =>
            {
                PipePrice = SetPipePrice(PipeUnitPrice, value, Product);
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

        partial void OnSelectedCustomerChanged(CustomerDto? value)
        {
            _onQuoteInputChangedTask = Task.Run(() =>
            {
                CustomerDiscount = SetCustomerDiscount(SelectedCustomer);
            });
            
        }

        #region Change TotalPriceBeforeDiscount
        public void SetTotalPriceBeforeDiscount()
        {
            TotalPriceBeforeDiscount = PipePrice + PipeTopPrice + EndPlatePrice;

            TotalPriceBeforeDiscount += SocketInputs.Sum(item => item.TeethPrice);
            TotalPriceBeforeDiscount += NippleInputs.Sum(item => item.TeethPrice);
        }

        partial void OnPipePriceChanged(decimal oldValue, decimal newValue)
        {
            _onQuoteInputChangedTask = RunAsync(async () =>
            {
                SetTotalPriceBeforeDiscount();

                // PipeTop
                var Result = await SetPipeTopAsync(Product, SelectedPipeDiameter, newValue);
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

        partial void OnEndPlatePriceChanged(decimal oldValue, decimal newValue)
        {
            _onQuoteInputChangedTask = Task.Run(() =>
            {
                SetTotalPriceBeforeDiscount();
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

        #region Socket and Nipple Inputs
        [RelayCommand]
        private async Task AddSocket()
        {
            try
            {
                var newInput = new TeethInputVM("SocketWaterMeter", this);
                await newInput.InitializeAsync(); // Wait after viewmodel is built

                // 確保 UI thread 修改 ObservableCollection
                Application.Current.Dispatcher.Invoke(() =>
                {
                    SocketInputs.Add(newInput);

                    if (SocketInputs.Count == 1)
                        SocketRowHeight = new GridLength(60);
                    else
                        SocketRowHeight = new GridLength(SocketRowHeight.Value + 40);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding socket input");
            }
        }

        [RelayCommand]
        private async Task AddNipple()
        {
            var newInput = new TeethInputVM("Nipple", this);
            await newInput.InitializeAsync(); // Wait after viewmodel is built

            // 確保 UI thread 修改 ObservableCollection
            Application.Current.Dispatcher.Invoke(() =>
            {
                NippleInputs.Add(newInput);

                if (NippleInputs.Count == 1)
                    NippleRowHeight = new GridLength(60);
                else
                    NippleRowHeight = new GridLength(NippleRowHeight.Value + 40);
            });
        }

        [RelayCommand]
        private void RemoveSocket(TeethInputVM item)
        {
            if (SocketInputs.Count <= 1)
                return;

            SocketInputs.Remove(item);

            SocketRowHeight = new GridLength(SocketRowHeight.Value - 40);
        }

        [RelayCommand]
        private void RemoveNipple(TeethInputVM item)
        {
            if (NippleInputs.Count <= 1)
                return;

            NippleInputs.Remove(item);

            NippleRowHeight = new GridLength(NippleRowHeight.Value - 40);
        }
        #endregion


        [RelayCommand]
        public override async Task ClearQuotation()
        {
            // Clear all properties
            EditingRecord = new WaterMeterEditDto();
            Product = new ProductDto();
            PipeDiameterList.Clear();
            SelectedPipeDiameter = null;
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
            Application.Current.Dispatcher.Invoke(() => SocketInputs.Clear()); // Cannot modify UI collection directly from thread pool
            Application.Current.Dispatcher.Invoke(() => NippleInputs.Clear());
            SocketRowHeight = new GridLength(60);
            NippleRowHeight = new GridLength(60);
            SocketDiameterList.Clear();
            SelectedSocketDiameter = new PipeDiameterDto();
            IsEndPlateEnabled = false;
            IsEndPlateChecked = false;
            EndPlatePrice = 0;
            IsEndPlatePriceVisible = Visibility.Hidden;
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
            // Validate inputs
            if (SelectedPipeDiameter == null || SelectedPipeThickness == null || SelectedPipeTop == null || PipeLength <= 0 || SelectedPipeTop == null || SelectedCustomer == null)
            {
                await _errorDialogService.ShowMessageAsync("FillInAllRequiredFieldsMessage");
                return;
            }
            if (_appStateService.User == null)
            {
                await _errorDialogService.ShowMessageAsync("LoginRequiredMessage");
                return;
            }

            // Create Nipple DTOs
            List<WaterMeterNippleEditDto> nipplesDTO = new();
            if (!(NippleInputs.Count() == 1 && NippleInputs[0].SelectedTeethDiameter == null) || !(NippleInputs.Count() <= 1))
            {
                nipplesDTO = NippleInputs
                    .Where(i => i.SelectedTeethDiameter != null && i.TeethQuantity >= 1)
                    .Select(i => new WaterMeterNippleEditDto
                    {
                        TeethID = IsEditMode == true ? i.TeethID : 0,
                        WaterMeterID = IsEditMode == true ? i.WaterMeterID : 0,

                        TeethDiameterID = i.SelectedTeethDiameter != null ? i.SelectedTeethDiameter.DiameterID : 0,
                        UnitPrice = i.TeethUnitPrice,
                        Quantity = i.TeethQuantity,
                        Price = i.TeethPrice
                    }).ToList();
            }

            // Create Socket DTOs
            List<WaterMeterSocketEditDto> socketDTO = new();
            if (!(SocketInputs.Count() == 1 && SocketInputs[0].SelectedTeethDiameter == null) || !(SocketInputs.Count() <= 1))
            {
                socketDTO = SocketInputs
                    .Where(i => i.SelectedTeethDiameter != null && i.TeethQuantity >= 1)
                    .Select(i => new WaterMeterSocketEditDto
                {
                    TeethID = IsEditMode == true ? i.TeethID : 0,
                    WaterMeterID = IsEditMode == true ? i.WaterMeterID : 0,

                    TeethDiameterID = i.SelectedTeethDiameter != null ? i.SelectedTeethDiameter.DiameterID : 0,
                    UnitPrice = i.TeethUnitPrice,
                    Quantity = i.TeethQuantity,
                    Price = i.TeethPrice
                }).ToList();
            }

            // Create WaterMeterEditDto
            var quotationDto = new WaterMeterEditDto
            {
                WaterMeterID = IsEditMode == true ? EditingRecord?.WaterMeterID ?? 0 : 0,
                Date = IsEditMode == true ? EditingRecord?.Date ?? DateTime.Now : DateTime.Now,

                PipeDiameterID = SelectedPipeDiameter.DiameterID,
                PipeThicknessID = SelectedPipeThickness.ThicknessID,
                PipeUnitPrice = PipeUnitPrice,
                PipeLength = PipeLength,
                PipePrice = PipePrice,
                PipeTopID = SelectedPipeTop.PipeTopID,
                PipeTopUnitPrice = PipeTopUnitPrice == 0 ? null : PipeTopUnitPrice,
                PipeTopQuantity = PipeTopQuantity == 0 ? null : PipeTopQuantity,
                PipeTopPrice = PipeTopPrice == 0 ? null : PipeTopPrice,
                EndPlate = IsEndPlateChecked,
                EndPlatePrice = IsEndPlateChecked == true ? EndPlatePrice : null,
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
            
            quotationDto.WaterMeterRecordNipples = nipplesDTO;
            quotationDto.WaterMeterRecordSockets = socketDTO;

            // Save quotation
            if (IsEditMode) // Edit an existing record
            {
                await _quoteWriteService.EditWaterMeterRecordAsync(quotationDto);
                _logger.LogInformation("Record edited successfully.");
            }
            else // Create a new record
            {
                await _quoteWriteService.AddWaterMeterRecordAsync(quotationDto);
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

            await _quoteWriteService.DeleteAllRecordsAsync<WaterMeterEditDto>();

            var result = await RefreshViewListAsync(RecordCache);
            ViewList = result.viewList;
            PageCountText = result.pageCountText;
        }

        [RelayCommand]
        public override async Task DeleteSelectedRecords()
        {
            // Delete selected records
            List<int> selectedRecordIDs = ViewList.Where(u => u.IsSelected).Select(u => u.WaterMeterID).ToList();

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

            await _quoteWriteService.DeleteRecordsByIDAsync<WaterMeterEditDto>(selectedRecordIDs);

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
            List<int> selectedRecordIDs = ViewList.Where(u => u.IsSelected).Select(u => u.WaterMeterID).ToList();

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
                    List<int> selectedRecordIDs = ViewList.Where(u => u.IsSelected).Select(u => u.WaterMeterID).ToList();

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
                    EditingRecord = await _quoteQueryService.GetWaterMeterRecordByIdAsync(selectedRecordIDs.First());
                    if (EditingRecord == null)
                    {
                        _logger.LogError("Selected record not found in database.");
                        return;
                    }

                    #region Fill in the fields
                    // Pipe
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
                    int i = 0; 
                    foreach (var socket in EditingRecord.WaterMeterRecordSockets)
                    {
                        if (i >= 1)
                            await AddSocket();
                        var input = SocketInputs[i];
                        await input.SetAndWaitAsync(() => input.WaterMeterID = EditingRecord.WaterMeterID);
                        await input.SetAndWaitAsync(() => input.TeethID = socket.TeethID);
                        await input.SetAndWaitAsync(() => input.SelectedTeethDiameter = input.TeethDiameterList.Where(d => d.DiameterID == socket.TeethDiameterID).FirstOrDefault());
                        await input.SetAndWaitAsync(() => input.TeethUnitPrice = socket.UnitPrice);
                        await input.SetAndWaitAsync(() => input.TeethQuantity = socket.Quantity);
                        await input.SetAndWaitAsync(() => input.TeethPrice = socket.Price);
                        i++;
                    }
                    // Nipple
                    int j = 0; 
                    foreach (var nipple in EditingRecord.WaterMeterRecordNipples)
                    {
                        if (j >= 1)
                            await AddNipple();
                        var input = NippleInputs[j];
                        await input.SetAndWaitAsync(() => input.WaterMeterID = EditingRecord.WaterMeterID);
                        await input.SetAndWaitAsync(() => input.TeethID = nipple.TeethID);
                        await input.SetAndWaitAsync(() => input.SelectedTeethDiameter = input.TeethDiameterList.Where(d => d.DiameterID == nipple.TeethDiameterID).FirstOrDefault());
                        await input.SetAndWaitAsync(() => input.TeethUnitPrice = nipple.UnitPrice);
                        await input.SetAndWaitAsync(() => input.TeethQuantity = nipple.Quantity);
                        await input.SetAndWaitAsync(() => input.TeethPrice = nipple.Price);
                        j++;
                    }
                    // EndPlate
                    await SetAndWaitAsync(() => IsEndPlateChecked = EditingRecord.EndPlate);
                    await SetAndWaitAsync(() => EndPlatePrice = EditingRecord.EndPlatePrice ?? 0);
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

        //public async Task SaveEdit()
        //{
        //    // Confirm Save
        //    var MessageBoxResult = MessageBox.Show("確定要儲存編輯?", "儲存編輯", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        //    if (MessageBoxResult == MessageBoxResult.No)
        //        return;

        //    // Save the edited quotation
        //    await SaveQuotation();

        //    IsEditMode = false;
        //}

        public override async Task SetAndWaitAsync(Action setAction)
        {
            // Ensure the action finished running on the UI thread
            setAction();

            if (_onQuoteInputChangedTask != null)
                await _onQuoteInputChangedTask;
        }

    }


    public partial class TeethInputVM : ViewModelBase
    {
        [ObservableProperty] public int waterMeterID = 0;
        [ObservableProperty] public int teethID = 0;
        [ObservableProperty] public List<PipeDiameterDto> teethDiameterList = new();
        [ObservableProperty] public PipeDiameterDto? selectedTeethDiameter = null;
        [ObservableProperty] public decimal teethUnitPrice = new();
        [ObservableProperty] public int teethQuantity = new();
        [ObservableProperty] public decimal teethPrice = new();
        private string componentName = "";
        private WaterMeterQuotationVM quotationVM;
        public Task? _onQuoteInputChangedTask;

        public TeethInputVM(string componentName, WaterMeterQuotationVM quotationVM) // SocketWaterMete or Nipple
        {
            this.componentName = componentName;
            this.quotationVM = quotationVM;
        }

        public override async Task InitializeAsync()
        {
            await LoadTeethDiameterAsync();
        }

        public async Task LoadTeethDiameterAsync()
        {
            // Set TeethUnitPrice
            TeethDiameterList = await _quoteQueryService.GetComponentDiametersByNameAsync(componentName);
        }

        partial void OnSelectedTeethDiameterChanged(PipeDiameterDto? value)
        {
            _onQuoteInputChangedTask = RunAsync(async () =>
            {
                if (value != null)
                {
                    // Set TeethUnitPrice
                    TeethUnitPrice = await _quoteQueryService.GetComponentPriceByNameAndDiameterAsync(componentName, value.DiameterID);
                }
                else
                {
                    TeethUnitPrice = 0;
                }
            });
        }

        partial void OnTeethUnitPriceChanged(decimal value)
        {
            _onQuoteInputChangedTask = Task.Run(() =>
            {
                if (value != 0)
                {
                    TeethQuantity = 1;
                    TeethPrice = 0;
                    TeethPrice = Math.Round(value * TeethQuantity, 2);
                }
                else
                {
                    TeethQuantity = 0;
                    TeethPrice = 0;
                }
            });
        }

        partial void OnTeethQuantityChanged(int value)
        {
            _onQuoteInputChangedTask = Task.Run(() =>
            {
                if (TeethUnitPrice != 0)
                {
                    TeethPrice = 0;
                    TeethPrice = Math.Round(TeethUnitPrice * value, 2);
                }
                else
                {
                    TeethPrice = 0;
                }
            });
        }

        partial void OnTeethPriceChanged(decimal oldValue, decimal newValue)
        {
            _onQuoteInputChangedTask = Task.Run(() =>
            {
                quotationVM.SetTotalPriceBeforeDiscount();
            });
        }

        public async Task SetAndWaitAsync(Action setAction)
        {
            setAction();

            if (_onQuoteInputChangedTask != null)
                await _onQuoteInputChangedTask;
        }
    }

}
