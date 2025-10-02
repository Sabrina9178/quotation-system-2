using QuotationSystem2.ApplicationLayer.DTOs;
using QuotationSystem2.PresentationLayer.ViewModels;
using System;
using System.Collections.Generic;
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
    /// CreateOrEditUserWindow.xaml 的互動邏輯
    /// </summary>
    public partial class CreateOrEditUserWindow : Window
    {
        public CreateOrEditUserVM ViewModel => (CreateOrEditUserVM)DataContext;

        public CreateOrEditUserWindow(bool createMode, EmployeeDto? dto = null)
        {
            InitializeComponent();

            var vm = new CreateOrEditUserVM(createMode, dto);
            vm.RequestCloseAction = Close; // 指定視窗關閉邏輯
            DataContext = vm;
        }
    }
}
