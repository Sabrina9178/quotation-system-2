using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace QuotationSystem2.Resources.Style
{
    public partial class Text : ResourceDictionary
    {
        public Text()
        {
            InitializeComponent();
        }

        // TextBox input inspection
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string newText = textBox.Text.Insert(textBox.SelectionStart, e.Text);

                // Check if the input is a valid number (up to two decimal places)
                Regex regex = new Regex(@"^\d*\.?\d{0,2}$");
                e.Handled = !regex.IsMatch(newText);
            }
        }
    }
}