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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Text.RegularExpressions;
using MD = MaterialDesignThemes.Wpf;
using WPF_PizzaDelivery.Util;

namespace WPF_PizzaDelivery
{
    /*
    public class MarginSetter
    {
        public static readonly DependencyProperty marginProperty =
            DependencyProperty.RegisterAttached("Margin", typeof(Thickness), typeof(MarginSetter), new UIPropertyMetadata(new Thickness(), onMarginChanged));

        public static Thickness getMargin(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(marginProperty);
        }

        public static void setMargin(DependencyObject obj, Thickness value)
        {
            obj.SetValue(marginProperty, value);
        }

        public static void onMarginChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var panel = sender as Panel;

            if (panel == null) return;

            panel.Loaded += new RoutedEventHandler(onLoad);
        }

        static void onLoad(object sender, RoutedEventArgs e)
        {
            var panel = sender as Panel;

            foreach (var child in panel.Children)
            {
                var fe = child as FrameworkElement;

                if (fe == null) continue;

                fe.Margin = MarginSetter.getMargin(panel);
            }
        }
    }
    
    public class Util
    {
        public static T querySelector<T>(DependencyObject top, Func<T, bool> rule) where T : DependencyObject
        {
            if (top is T)
                return top as T;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(top); i++)
            {
                var child = VisualTreeHelper.GetChild(top, i);

                if (child is T)
                {
                    var tChild = child as T;

                    if (rule(tChild))
                        return tChild;
                }

                return querySelector(child, rule);
            }

            return top as T;
        }

        public List<T> querySelectorAll<T>(DependencyObject top, Func<T, bool> rule) where T : DependencyObject
        {

        }
    }
    

    public struct Order
    {
        public int id { get; set; }
        public int basePrice { get; set; }
        public int totalPrice { get; set; }
        public int count { get; set; }
    }
    */

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            load();
        }

        void load()
        {
            loadMenuPage();
            loadCartPage();
        }

        void loadMenuPage()
        {
            var table = FindName("Menu_GRD_Pizzas") as Grid;
            table.Children.Clear();

            for (int i = 0; i < 10; i++)
            {
                var row = new RowDefinition { Height = new GridLength(300) };

                table.RowDefinitions.Add(row);
            }

            for (int i = 0; i < table.RowDefinitions.Count; i++)
            {
                for (int j = 0; j < table.ColumnDefinitions.Count; j++)
                {
                    //var pizza = XamlReader.Parse(XamlWriter.Save(grd)) as Grid;

                    var pizza = new Grid
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Width = 200,
                        Height = 250
                    };

                    {
                        pizza.RowDefinitions.Add(new RowDefinition { Height = new GridLength(200) });
                        pizza.RowDefinitions.Add(new RowDefinition { });

                        var btn = new Button
                        {
                            Width = 200,
                            Height = 200,
                            Style = FindResource("MaterialDesignFlatLightButton") as Style
                        };

                        pizza.Children.Add(btn);

                        {
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
                                    Source = new BitmapImage(new Uri(Media.directory + Media.pizzaNameToFileName["Песто"] + ".png", UriKind.Absolute)),
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    VerticalAlignment = VerticalAlignment.Center,
                                    Width = 145,
                                    Height = 145,
                                    Stretch = Stretch.UniformToFill
                                };

                                grid.Children.Add(img);

                                var lbl = new Label
                                {
                                    Content = "Ветчина и сыр",
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    VerticalAlignment = VerticalAlignment.Center,
                                    FontWeight = FontWeights.SemiBold
                                };

                                Grid.SetRow(lbl, 1);
                                grid.Children.Add(lbl);

                                lbl = new Label
                                {
                                    Content = "от 389 р.",
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    VerticalAlignment = VerticalAlignment.Center
                                };

                                Grid.SetRow(lbl, 2);
                                grid.Children.Add(lbl);
                            }
                        }

                        btn = new Button
                        {
                            Content = "Выбрать"
                        };

                        Grid.SetRow(btn, 1);
                        pizza.Children.Add(btn);
                    }

                    Grid.SetRow(pizza, i);
                    Grid.SetColumn(pizza, j);
                    table.Children.Add(pizza);
                }
            }
        }

        void loadCartPage()
        {
            var stack = FindName("Cart_SP_Orders") as StackPanel;

            stack.Children.Clear();

            for (int i = 0; i < 10; i++)
            {
                var card = new MD.Card
                {
                    Width = 500,
                    Height = 150,
                    Padding = new Thickness(10)
                };

                stack.Children.Add(card);

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
                                    Source = new BitmapImage(new Uri("HamNCheese.png", UriKind.Relative)),
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
                                    Content = "Ветчина и сыр",
                                    FontWeight = FontWeights.Bold,
                                    FontSize = 20
                                };

                                grid2.Children.Add(lbl);

                                lbl = new Label
                                {
                                    Content = "30 см, традиционное тесто",
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

                            var lbl = new Label
                            {
                                Name = "Order_LBL_Price",
                                Content = "389 р.",
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Center,
                                FontWeight = FontWeights.Bold,
                                FontSize = 18
                            };

                            grid1.Children.Add(lbl);

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
                                btn.Click += Order_BTN_Minus_onClick;

                                grid2.Children.Add(btn);

                                lbl = new Label
                                {
                                    Name = "Order_LBL_PizzaCount",
                                    Content = "1",
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    VerticalAlignment = VerticalAlignment.Center,
                                    FontWeight = FontWeights.Bold,
                                    FontSize = 18
                                };

                                Grid.SetColumn(lbl, 1);
                                grid2.Children.Add(lbl);

                                btn = new Button
                                {
                                    Content = new MD.PackIcon { Kind = MD.PackIconKind.Plus },
                                    Style = FindResource("MaterialDesignFlatButton") as Style,
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    VerticalAlignment = VerticalAlignment.Center,
                                    Padding = new Thickness(0),
                                    Width = 30
                                };
                                btn.Click += Order_BTN_Plus_onClick;

                                Grid.SetColumn(btn, 2);
                                grid2.Children.Add(btn);
                            }
                        }
                    }
                }
            }
        }

        void updatePrices()
        {
            var lbl_total = FindName("Cart_LBL_TotalPrice") as Label;
            var sp_orders = FindName("Cart_SP_Orders") as StackPanel;
            var labels = sp_orders.Children.OfType<Label>();

            var total = 0;
            var prices = labels.Where(e => e.Name == "Order_LBL_Price").ToArray();
            var counts = labels.Where(e => e.Name == "Order_LBL_PizzaCount").ToArray();

            for (int i = 0; i < prices.Length; i++)
            {
                var price = int.Parse(prices[i].Content as string);
                var count = int.Parse(counts[i].Content as string);

                total += price * count;
            }

            lbl_total.Content = total.ToString();
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

        private void Order_BTN_Minus_onClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var lbl_count = ((Grid)btn.Parent).Children.OfType<Label>().First();

            var count = int.Parse(lbl_count.Content as string);

            if (count > 0)
            {
                count--;

                lbl_count.Content = count.ToString();

                updatePrices();
            }
        }

        private void Order_BTN_Plus_onClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var lbl_count = ((Grid)btn.Parent).Children.OfType<Label>().First();

            var count = int.Parse(lbl_count.Content as string);

            count++;

            lbl_count.Content = count.ToString();

            updatePrices();
        }
    }
}
