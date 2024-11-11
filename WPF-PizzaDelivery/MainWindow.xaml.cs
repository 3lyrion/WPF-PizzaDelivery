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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace WPF_PizzaDelivery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string PHONE_NUMBER_ALPHABET = "1234567890+";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Login_TB_PhoneNumber_onTextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;

            tb.Text = Regex.Replace(tb.Text, "[^0-9+]", "");
        }
    }
}
