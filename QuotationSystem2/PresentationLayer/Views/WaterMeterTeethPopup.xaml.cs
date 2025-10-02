using CommunityToolkit.Mvvm.ComponentModel;
using QuotationSystem2.ApplicationLayer.Dtos;
using QuotationSystem2.PresentationLayer.Services.Interfaces;
using QuotationSystem2.PresentationLayer.Services.Localization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuotationSystem2.PresentationLayer.Views
{
    /// <summary>
    /// WaterMeterTeethPopup.xaml 的互動邏輯
    /// </summary>
    public partial class WaterMeterTeethPopup : Window
    {
        public ObservableCollection<WaterMeterTeethViewDto> TeethCollection = new();

        private ILanguageService _languageService = LanguageService.Instance;
        public WaterMeterTeethPopup(ObservableCollection<WaterMeterTeethViewDto> records)
        {
            InitializeComponent();
            records = ChangeTeethTypeLanguage(records);
            TeethCollection = records;
            TeethDataGrid.ItemsSource = TeethCollection;
            this.DataContext = this;

            _languageService.LanguageChanged += OnLanguageChanged;
        }

        public ObservableCollection<WaterMeterTeethViewDto> ChangeTeethTypeLanguage(ObservableCollection<WaterMeterTeethViewDto> records)
        {
            foreach (var record in records)
            {
                if (record.TeethType == "Nipple")
                {
                    record.TeethType = _languageService.GetUIStrings("Nipple");
                }
                else if (record.TeethType == "Socket")
                {
                    record.TeethType = _languageService.GetUIStrings("Socket");
                }
            }
            return records;
        }

        private void OnLanguageChanged(object? sender, EventArgs e)
        {
            // Refresh PipeTopList
            TeethCollection = ChangeTeethTypeLanguage(TeethCollection);
        }
    }
}
