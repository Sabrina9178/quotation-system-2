using Microsoft.Extensions.DependencyInjection;
using QuotationSystem2.PresentationLayer.ViewModels;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotationSystem2.PresentationLayer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = App.AppHost.Services.GetRequiredService<MainViewModel>();
        }
        
    }
}
