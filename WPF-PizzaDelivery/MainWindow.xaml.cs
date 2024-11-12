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

namespace WPF_PizzaDelivery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var table = FindName("Menu_GRD_Pizzas") as Grid;
//            var grd = table.FindName("Menu_GRD_Pizza") as Grid;
//            grd.Visibility = Visibility.Hidden;


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
                                    Source = new BitmapImage(new Uri("HamNCheese.png", UriKind.Relative)),
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

            private void Login_TB_PhoneNumber_onTextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;

            tb.Text = Regex.Replace(tb.Text, "[^0-9+]", "");
        }
    }
}
