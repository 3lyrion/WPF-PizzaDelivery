using System;
using System.Globalization;
using System.Collections.Generic;
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
using MD = MaterialDesignThemes.Wpf;
using DTO = Interfaces.DTO;
using SV = Interfaces.Service;
using WPF_PizzaDelivery.Util;

namespace WPF_PizzaDelivery
{
    public class PizzaImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
                return Media.directory + Media.pizzaNameToFileName[value as string] + ".png";

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PizzaMenuPriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal)
                return "от " + string.Format("{0:C2}", (decimal)value);

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public partial class MainWindow : Window
    {
        SV.IClient clientService;
        SV.ICourier courierService;
        SV.IDough doughService;
        SV.IOrder orderService;
        SV.IPizza pizzaService;
        SV.IPizza_Order pizzaOrderService;
        SV.IPizza_Size pizzaSizeService;
        SV.IReport reportService;

        List<DTO.Dough> allDough;
        List<DTO.Order> allOrders;
        List<DTO.Pizza> allPizzas;
        List<DTO.Pizza_Order> allPizzaOrders;
        List<DTO.Pizza_Size> allPizzaSizes;

        DTO.Order curOrder;

        FrameworkElement menuPage;
        FrameworkElement cartPage;
        FrameworkElement profilePage;

        delegate void PizzaOrderDeletion(DTO.Pizza_Order poDto);
        event PizzaOrderDeletion? pizzaOrderDeletion;

        public MainWindow(
            SV.IClient theClientService,
            SV.ICourier theCourierService,
            SV.IDough theDoughService,
            SV.IOrder theOrderService,
            SV.IPizza thePizzaService,
            SV.IPizza_Order thePizzaOrderService,
            SV.IPizza_Size thePizzaSizeService,
            SV.IReport theReportService
        )
        {
            clientService = theClientService;
            courierService = theCourierService;
            doughService = theDoughService;
            orderService = theOrderService;
            pizzaService = thePizzaService;
            pizzaOrderService = thePizzaOrderService;
            pizzaSizeService = thePizzaSizeService;
            reportService = theReportService;

            InitializeComponent();

            load();
        }

        void load()
        {
            loadMembers();
            loadMenuPage();
            loadCartPage();
        }

        void loadMembers()
        {
            allDough = doughService.getAllDough();
            allOrders = orderService.getAllOrders();
            allPizzaOrders = pizzaOrderService.getAllPO();
            allPizzas = pizzaService.getAllPizzas();
            allPizzaSizes = pizzaSizeService.getAllSizes();

            menuPage = FindName("Page_Menu") as FrameworkElement;
            cartPage = FindName("Page_Cart") as FrameworkElement;
            profilePage = FindName("Page_Profile") as FrameworkElement;
        }

        void loadMenuPage()
        {
            var ic = menuPage.FindName("Menu_IC_Pizzas") as ItemsControl;
            ic.ItemsSource = allPizzas;

            /*
            var table = menuPage.FindName("Menu_GRD_Pizzas") as Grid;
            table.Children.Clear();

            var COL_CAP = table.ColumnDefinitions.Count;

            int i = 0;
            int j = 0;

            foreach (var pizzaDto in allPizzas)
            {
                var card = new MD.Card
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 250,
                    Height = 275,
                    Padding = new Thickness(25, 10, 25, 0)
                };
                
            
            var pizza = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 200,
                Height = 250,
            };

            //card.Content = pizza;

            Grid.SetRow(pizza, i);
            Grid.SetColumn(pizza, j);
            table.Children.Add(pizza);

            {
                pizza.RowDefinitions.Add(new RowDefinition { Height = new GridLength(200) });
                pizza.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

                var btn = new Button
                {
                    Width = 200,
                    Height = 200,
                    Style = FindResource("MaterialDesignFlatLightButton") as Style
                };

                pizza.Children.Add(btn);

                var grid = new Grid
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 200,
                    Height = 200
                };

                btn.Content = grid;

                {
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(150) });
                    grid.RowDefinitions.Add(new RowDefinition { });
                    grid.RowDefinitions.Add(new RowDefinition { });

                    var img = new Image
                    {
                        Source = new BitmapImage(new Uri(Media.directory + Media.pizzaNameToFileName[pizzaDto.name] + ".png", UriKind.Relative)),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Width = 145,
                        Height = 145,
                        Stretch = Stretch.UniformToFill
                    };

                    grid.Children.Add(img);

                    var lbl = new Label
                    {
                        Content = pizzaDto.name,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontWeight = FontWeights.SemiBold
                    };

                    Grid.SetRow(lbl, 1);
                    grid.Children.Add(lbl);

                    lbl = new Label
                    {
                        Content = "от " + string.Format("{0:C2}", pizzaDto.cost),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };

                    Grid.SetRow(lbl, 2);
                    grid.Children.Add(lbl);
                }

                var btn_select = new Button
                {
                    Content = "Выбрать"
                };

                Grid.SetRow(btn_select, 1);
                pizza.Children.Add(btn_select);

                grid = new Grid { Visibility = Visibility.Hidden };
                Grid.SetRow(grid, 1);
                pizza.Children.Add(grid);

                Label lbl_quantity;
                Button btn_minus;
                Button btn_plus;

                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition { });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { });

                    btn_minus = new Button
                    {
                        Content = new MD.PackIcon { Kind = MD.PackIconKind.Minus },
                        Style = FindResource("MaterialDesignFlatButton") as Style,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Padding = new Thickness(0),
                        Width = 30
                    };

                    grid.Children.Add(btn_minus);

                    lbl_quantity = new Label
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontWeight = FontWeights.Bold,
                        FontSize = 18
                    };

                    Grid.SetColumn(lbl_quantity, 1);
                    grid.Children.Add(lbl_quantity);

                    btn_plus = new Button
                    {
                        Content = new MD.PackIcon { Kind = MD.PackIconKind.Plus },
                        Style = FindResource("MaterialDesignFlatButton") as Style,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Padding = new Thickness(0),
                        Width = 30
                    };

                    Grid.SetColumn(btn_plus, 2);
                    grid.Children.Add(btn_plus);
                }

                btn_select.Click += (s, e) =>
                {
                    var poDto = new DTO.Pizza_Order
                    {
                        dough_id = 1,
                        pizza_id = pizzaDto.id,
                        order_id = curOrder.id,
                        quantity = 1,
                        size_id = 1
                    };

                    lbl_quantity.Content = poDto.quantity.ToString();
                    poDto.quantityChange += () =>
                    {
                        lbl_quantity.Content = poDto.quantity.ToString();
                    };

                    btn_minus.Click += (s, e) =>
                    {
                        decreasePizzaOrderQuantity(poDto);
                        updateOrderPrice();
                    };

                    btn_plus.Click += (s, e) =>
                    {
                        increasePizzaOrderQuantity(poDto);
                        updateOrderPrice();
                    };

                    addPizzaOrder(poDto);

                    btn_select.Visibility = Visibility.Hidden;
                    grid.Visibility = Visibility.Visible;
                };

                pizzaOrderDeletion += (poDto_) =>
                {
                    if (pizzaDto.id != poDto_.pizza_id) return;

                    btn_select.Visibility = Visibility.Visible;
                    grid.Visibility = Visibility.Hidden;
                };
            }

            if (j + 1 >= COL_CAP)
            {
                j = 0;

                i++;

                // Если это был последний элемент, то новой строки не будет
                if (COL_CAP * (i + 1) != allPizzas.Count) 
                    table.RowDefinitions.Add(new RowDefinition { Height = new GridLength(300) });
            }

            else j++;
        }*/
    }

        void loadCartPage()
        {
            var stack = cartPage.FindName("Cart_SP_Orders") as StackPanel;
            stack.Children.Clear();

            curOrder = allOrders.First();
            var poDtos = allPizzaOrders.FindAll(e => e.order_id == curOrder.id);

            allPizzaOrders.Clear();

            foreach (var poDto in poDtos)
                addPizzaOrder(poDto);
        }

        void updatePizzaOrderPrice(DTO.Pizza_Order poDto)
        {
            var pizzaDto = allPizzas.First(e => e.id == poDto.pizza_id);
            var sizeDto = allPizzaSizes.First(e => e.id == poDto.size_id);

            poDto.cost = pizzaDto.cost * sizeDto.cost_mult * poDto.quantity;
        }

        void updateOrderPrice()
        {
            decimal total = 0.0m;

            foreach (var poDto in allPizzaOrders.FindAll(e => e.order_id == curOrder.id))
                total += poDto.cost;

            curOrder.cost = total;
        }

        void addPizzaOrder(DTO.Pizza_Order poDto)
        {
            allPizzaOrders.Add(poDto);

            var stack = cartPage.FindName("Cart_SP_Orders") as StackPanel;

            var doughDto = allDough.First(e => e.id == poDto.dough_id);
            var pizzaDto = allPizzas.First(e => e.id == poDto.pizza_id);
            var sizeDto = allPizzaSizes.First(e => e.id == poDto.size_id);

            var card = new MD.Card
            {
                Width = 500,
                Height = 150,
                Padding = new Thickness(10)
            };

            stack.Children.Add(card);
            pizzaOrderDeletion += (poDto_) =>
            {
                if (poDto != poDto_) return;

                allPizzaOrders.Remove(poDto);

                stack.Children.Remove(card);

                updateOrderPrice();
            };

            {
                var grid = new Grid { };

                card.Content = grid;

                {
                    grid.RowDefinitions.Add(new RowDefinition { MinHeight = 100 });
                    grid.RowDefinitions.Add(new RowDefinition { });

                    var grid1 = new Grid { };
                    grid.Children.Add(grid1);

                    {
                        grid1.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });
                        grid1.ColumnDefinitions.Add(new ColumnDefinition { });
                        grid1.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });

                        var bd = new Border { Padding = new Thickness(7.5) };
                        {
                            var img = new Image
                            {
                                Source = new BitmapImage(new Uri(Media.directory + Media.pizzaNameToFileName[pizzaDto.name] + ".png", UriKind.Relative)),
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center
                            };

                            bd.Child = img;
                        }

                        grid1.Children.Add(bd);

                        var grid2 = new Grid { };

                        Grid.SetColumn(grid2, 1);
                        grid1.Children.Add(grid2);

                        {
                            grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
                            grid2.RowDefinitions.Add(new RowDefinition { });

                            var lbl = new Label
                            {
                                Content = pizzaDto.name,
                                FontWeight = FontWeights.Bold,
                                FontSize = 20
                            };

                            grid2.Children.Add(lbl);

                            lbl = new Label
                            {
                                Content = $"{sizeDto.name} {sizeDto.size} см, {doughDto.name} тесто",
                                FontWeight = FontWeights.Bold,
                                FontSize = 13,
                                Foreground = new SolidColorBrush(Colors.Gray)
                            };

                            Grid.SetRow(lbl, 1);
                            grid2.Children.Add(lbl);
                        }

                        var btn = new Button
                        {
                            Content = new MD.PackIcon { Kind = MD.PackIconKind.Close },
                            Style = FindResource("MaterialDesignFlatButton") as Style,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Padding = new Thickness(0),
                            Margin = new Thickness(0, 0, 0, 45),
                            Width = 32,
                            Height = 32,
                            Foreground = new SolidColorBrush(Colors.Black)
                        };
                        btn.Click += (s, e) =>
                        {
                            pizzaOrderDeletion?.Invoke(poDto);
                        };

                        Grid.SetColumn(btn, 2);
                        grid1.Children.Add(btn);
                    }

                    grid1 = new Grid { };

                    Grid.SetRow(grid1, 1);
                    grid.Children.Add(grid1);

                    {
                        grid1.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(260) });
                        grid1.ColumnDefinitions.Add(new ColumnDefinition { });
                        grid1.ColumnDefinitions.Add(new ColumnDefinition { });

                        var lbl_price = new Label
                        {
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Center,
                            FontWeight = FontWeights.Bold,
                            FontSize = 15
                        };
                        poDto.costChange += () =>
                        {
                            lbl_price.Content = string.Format("{0:C2}", poDto.cost);
                        };
                        updatePizzaOrderPrice(poDto);

                        grid1.Children.Add(lbl_price);

                        var btn = new Button
                        {
                            Content = "Изменить",
                            Style = FindResource("MaterialDesignFlatButton") as Style,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            FontWeight = FontWeights.Bold,
                            FontSize = 16
                        };

                        Grid.SetColumn(btn, 1);
                        grid1.Children.Add(btn);

                        var grid2 = new Grid { };

                        Grid.SetColumn(grid2, 2);
                        grid1.Children.Add(grid2);

                        {
                            grid2.ColumnDefinitions.Add(new ColumnDefinition { });
                            grid2.ColumnDefinitions.Add(new ColumnDefinition { });
                            grid2.ColumnDefinitions.Add(new ColumnDefinition { });

                            btn = new Button
                            {
                                Content = new MD.PackIcon { Kind = MD.PackIconKind.Minus },
                                Style = FindResource("MaterialDesignFlatButton") as Style,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center,
                                Padding = new Thickness(0),
                                Width = 30
                            };

                            grid2.Children.Add(btn);

                            var lbl = new Label
                            {
                                Content = poDto.quantity.ToString(),
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center,
                                FontWeight = FontWeights.Bold,
                                FontSize = 18
                            };
                            poDto.quantityChange += () =>
                            {
                                lbl.Content = poDto.quantity.ToString();
                            };


                            Grid.SetColumn(lbl, 1);
                            grid2.Children.Add(lbl);

                            btn.Click += (s, e) =>
                            {
                                decreasePizzaOrderQuantity(poDto);
                                updateOrderPrice();
                            };

                            btn = new Button
                            {
                                Content = new MD.PackIcon { Kind = MD.PackIconKind.Plus },
                                Style = FindResource("MaterialDesignFlatButton") as Style,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center,
                                Padding = new Thickness(0),
                                Width = 30
                            };
                            btn.Click += (s, e) =>
                            {
                                increasePizzaOrderQuantity(poDto);
                                updateOrderPrice();
                            };

                            Grid.SetColumn(btn, 2);
                            grid2.Children.Add(btn);
                        }
                    }
                }
            }

            var lbl_total = FindName("Cart_LBL_TotalPrice") as Label;
            curOrder.costChange += () =>
            {
                lbl_total.Content = "Сумма заказа: " + string.Format("{0:C2}", curOrder.cost);
            };

            updateOrderPrice();
        }

        void decreasePizzaOrderQuantity(DTO.Pizza_Order poDto)
        {
            if (poDto.quantity > 1)
            {
                poDto.quantity--;

                updatePizzaOrderPrice(poDto);
            }

            else pizzaOrderDeletion?.Invoke(poDto);
        }

        void increasePizzaOrderQuantity(DTO.Pizza_Order poDto)
        {
            poDto.quantity++;

            updatePizzaOrderPrice(poDto);
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
