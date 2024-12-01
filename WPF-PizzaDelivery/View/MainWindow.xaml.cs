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
        const string PHONE_NUMBER_PATTERN = @"^\+(\d*)$";
        const string ADDRESS_PATTERN = @"^[а-яА-ЯёЁ,.\s]*$";
        const string RECIPIENT_NAME_PATTERN = @"^[а-яА-ЯёЁ\s]*$";

        public MainWindow()
        {
            var kernel = new StandardKernel(new Util.NinjectRegistrations(), new Util.ReposModule("PizzaDeliveryDB"));

            SV.IClient clientService = kernel.Get<SV.IClient>();
            SV.IDough doughService = kernel.Get<SV.IDough>();
            SV.IIngredient ingredientService = kernel.Get<SV.IIngredient>();
            SV.IOrder orderService = kernel.Get<SV.IOrder>();
            SV.IPizza pizzaService = kernel.Get<SV.IPizza>();
            SV.IPizzaOrder pizzaOrderService = kernel.Get<SV.IPizzaOrder>();
            SV.IPizzaSize pizzaSizeService = kernel.Get<SV.IPizzaSize>();
            SV.IRecipe recipeService = kernel.Get<SV.IRecipe>();

            DataContext = new ViewModel.App
            (
                clientService,
                doughService,
                ingredientService,
                orderService,
                pizzaService,
                pizzaOrderService,
                pizzaSizeService,
                recipeService
            );

            InitializeComponent();
        }

        private void Login_TB_PhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;

            if (tb.Text.Length > 0 && !Regex.IsMatch(tb.Text, PHONE_NUMBER_PATTERN))
            {
                tb.Text = tb.Text.Substring(0, tb.Text.Length - 1);
                tb.SelectionStart = tb.Text.Length;
            }
        }

        private void Checkout_TB_Address_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;

            if (tb.Text.Length > 0 && !Regex.IsMatch(tb.Text, ADDRESS_PATTERN))
            {
                tb.Text = tb.Text.Substring(0, tb.Text.Length - 1);
                tb.SelectionStart = tb.Text.Length;
            }
        }

        private void Checkout_TB_RecipientName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;

            if (tb.Text.Length > 0 && !Regex.IsMatch(tb.Text, RECIPIENT_NAME_PATTERN))
            {
                tb.Text = tb.Text.Substring(0, tb.Text.Length - 1);
                tb.SelectionStart = tb.Text.Length;
            }
        }
    }
}
