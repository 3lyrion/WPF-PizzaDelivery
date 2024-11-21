using System;
using System.Globalization;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Ninject;
using MD = MaterialDesignThemes.Wpf;
using DTO = Interfaces.DTO;
using SV = Interfaces.Service;

namespace PizzaDelivery.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var kernel = new StandardKernel(new Util.NinjectRegistrations(), new Util.ReposModule("PizzaDeliveryDB"));

            SV.IClient clientService = kernel.Get<SV.IClient>();
            SV.IDough doughService = kernel.Get<SV.IDough>();
            SV.IOrder orderService = kernel.Get<SV.IOrder>();
            SV.IPizza pizzaService = kernel.Get<SV.IPizza>();
            SV.IPizzaOrder pizzaOrderService = kernel.Get<SV.IPizzaOrder>();
            SV.IPizzaSize pizzaSizeService = kernel.Get<SV.IPizzaSize>();

            DataContext = new ViewModel.App
            (
                clientService,
                doughService,
                orderService,
                pizzaService,
                pizzaOrderService,
                pizzaSizeService
            );

            InitializeComponent();
        }

        private void Login_TB_PhoneNumber_onTextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;

            tb.Text = Regex.Replace(tb.Text, "[^0-9+]", "");
        }

        private void Login_BTN_Register_onClick(object sender, RoutedEventArgs e)
        {
            var vb_login = FindName("Profile_VB_Login") as Viewbox;
            var vb_reg = FindName("Profile_VB_Register") as Viewbox;

            vb_login.Visibility = Visibility.Hidden;
            vb_reg.Visibility = Visibility.Visible;
        }

        private void Register_BTN_Back_onClick(object sender, RoutedEventArgs e)
        {
            var vb_login = FindName("Profile_VB_Login") as Viewbox;
            var vb_reg = FindName("Profile_VB_Register") as Viewbox;

            vb_login.Visibility = Visibility.Visible;
            vb_reg.Visibility = Visibility.Hidden;
        }
    }
}
