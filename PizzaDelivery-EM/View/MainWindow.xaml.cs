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

namespace PizzaDelivery_EM.View
{
    public partial class MainWindow : Window
    {
        const string PHONE_NUMBER_PATTERN = @"^\+(\d*)$";

        public MainWindow()
        {
            var kernel = new StandardKernel(new Util.NinjectRegistrations(), new Util.ReposModule("PizzaDeliveryDB"));

            SV.ICook cookService = kernel.Get<SV.ICook>();
            SV.ICourier courierService = kernel.Get<SV.ICourier>();
            SV.IDough doughService = kernel.Get<SV.IDough>();
            SV.IIngredient ingredientService = kernel.Get<SV.IIngredient>();
            SV.IOrder orderService = kernel.Get<SV.IOrder>();
            SV.IPizza pizzaService = kernel.Get<SV.IPizza>();
            SV.IPizzaOrder pizzaOrderService = kernel.Get<SV.IPizzaOrder>();
            SV.IPizzaSize pizzaSizeService = kernel.Get<SV.IPizzaSize>();
            SV.IRecipe recipeService = kernel.Get<SV.IRecipe>();

            var viewModel = new ViewModel.App
            (
                cookService,
                courierService,
                doughService,
                ingredientService,
                orderService,
                pizzaService,
                pizzaOrderService,
                pizzaSizeService,
                recipeService
            );

            Closing += viewModel.OnWindowClosing;

            DataContext = viewModel;

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
    }
}
