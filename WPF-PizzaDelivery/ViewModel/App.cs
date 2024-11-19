using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
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
using MD = MaterialDesignThemes.Wpf;
using DTO = Interfaces.DTO;
using SV = Interfaces.Service;
using WPF_PizzaDelivery.Util;

namespace WPF_PizzaDelivery.ViewModel
{
    public class PriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal)
                return string.Format("{0:C2}", (decimal)value);

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PizzaImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
                return Media.directory + Media.pizzaNameToFileName[(string)value] + ".png";

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class OrderPartBottomLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Model.OrderPart)
            {
                var orderPart = value as Model.OrderPart;

                return $"{orderPart.SizeName} {orderPart.SizeValue} см, {orderPart.DoughName} тесто";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PizzaQuantityToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                var quantity = (int)value;

                if (quantity > 0) return Visibility.Hidden;
                return Visibility.Visible;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class InversePizzaQuantityToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                var quantity = (int)value;

                if (quantity > 0) return Visibility.Visible;
                return Visibility.Hidden;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class App : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

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

        public Model.Order CurrentOrder { get; set; }

        public ObservableCollection<Model.Pizza> Pizzas { get; set; }
        public ObservableCollection<Model.OrderPart> OrderParts { get; set; }

        RelayCommand incrPizzaQuantityCommand;
        public RelayCommand IncreasePizzaQuantityCommand
        {
            get
            {
                return incrPizzaQuantityCommand ??
                  (incrPizzaQuantityCommand = new RelayCommand(obj =>
                  {
                      if (obj is Model.Pizza)
                      {
                          var pizza = obj as Model.Pizza;

                          if (pizza == null) return;

                          pizza.Quantity++;

                          if (pizza.Quantity == 1)
                          {
                              var doughDto = allDough.First(e => e.id == 1);
                              var sizeDto = allPizzaSizes.First(e => e.id == 1);

                              var part = new Model.OrderPart();
                              part.PropertyChanged += (s, e) =>
                              {
                                  if (e.PropertyName == "Quantity")
                                  {
                                      part.Cost = pizza.Cost * part.Quantity * sizeDto.cost_mult;

                                      updateOrderCost();
                                  }
                              };
                              part.Name = pizza.Name;
                              part.Cost = pizza.Cost;
                              part.Quantity = pizza.Quantity;
                              part.DoughName = doughDto.name;
                              part.SizeName = sizeDto.name;
                              part.SizeValue = sizeDto.size;

                              bind(pizza, part);

                              OrderParts.Add(part);
                              updateOrderCost();
                          }
                      }

                      else if (obj is Model.OrderPart)
                      {
                          var part = obj as Model.OrderPart;

                          if (part == null) return;

                          part.Quantity++;
                      }

                  }));
            }
        }

        RelayCommand decrPizzaQuantityCommand;
        public RelayCommand DecreasePizzaQuantityCommand
        {
            get
            {
                return decrPizzaQuantityCommand ??
                  (decrPizzaQuantityCommand = new RelayCommand(obj =>
                  {
                      if (obj is Model.Pizza)
                      {
                          var pizza = obj as Model.Pizza;

                          if (pizza == null) return;

                          if (pizza.Quantity > 0)
                              pizza.Quantity--;

                          if (pizza.Quantity == 0)
                          {
                              var part = OrderParts.First(e => e.Name == pizza.Name);
                              OrderParts.Remove(part);
                          }
                      }

                      else if (obj is Model.OrderPart)
                      {
                          var part = obj as Model.OrderPart;

                          if (part == null) return;

                          if (part.Quantity > 0)
                              part.Quantity--;

                          if (part.Quantity == 0)
                              OrderParts.Remove(part);
                      }
                  }));
            }
        }

        RelayCommand removeOrderPartCommand;
        public RelayCommand RemoveOrderPartCommand
        {
            get
            {
                return removeOrderPartCommand ??
                  (removeOrderPartCommand = new RelayCommand(obj =>
                  {
                      if (obj is Model.OrderPart)
                      {
                          var part = obj as Model.OrderPart;

                          if (part == null) return;

                          part.Quantity = 0;
                          OrderParts.Remove(part);
                      }
                  }));
            }
        }

        public App(
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

            load();
        }

        void bind(Model.Pizza pizza, Model.OrderPart orderPart)
        {
            pizza.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "Quantity")
                    orderPart.Quantity = pizza.Quantity;
            };

            orderPart.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "Quantity")
                    pizza.Quantity = orderPart.Quantity;
            };
        }

        void updateOrderCost()
        {
            CurrentOrder.Cost = OrderParts.Sum(e => e.Cost);
        }

        void load()
        {
            loadMembers();
//            loadMenuPage();
//            loadCartPage();
        }

        void loadMembers()
        {
            allDough = doughService.getAllDough();
            allOrders = orderService.getAllOrders();
            allPizzaOrders = pizzaOrderService.getAllPO();
            allPizzas = pizzaService.getAllPizzas();
            allPizzaSizes = pizzaSizeService.getAllSizes();

            CurrentOrder = new Model.Order();

            Pizzas = new ObservableCollection<Model.Pizza>();
            
            foreach (var pizzaDto in allPizzas)
            {
                int quantity = 0;

                try
                {
                    var poDto = allPizzaOrders.First(e => e.pizza_id == pizzaDto.id && e.id == 1);

                    quantity = poDto.quantity;
                }

                catch
                {
                    
                }

                finally
                {
                    Pizzas.Add(new Model.Pizza
                    {
                        Name = pizzaDto.name,
                        Cost = pizzaDto.cost,
                        Quantity = 0
                    });
                }
            }
            
            OrderParts = new ObservableCollection<Model.OrderPart>();
            /*
            foreach (var poDto in allPizzaOrders.Where(e => e.id == 1))
            {
                var doughDto = allDough.First(e => e.id == poDto.dough_id);
                var pizzaDto = allPizzas.First(e => e.id == poDto.pizza_id);
                var sizeDto = allPizzaSizes.First(e => e.id == poDto.size_id);

                
                var part = new Model.OrderPart();
                part.Name = pizzaDto.name;
                part.Cost = pizzaDto.cost;
                part.Quantity = poDto.quantity;
                part.DoughName = doughDto.name;
                part.SizeName = sizeDto.name;
                part.SizeValue = sizeDto.size;
                
                OrderParts.Add(part);
            }
            */



        //    menuPage = FindName("Page_Menu") as FrameworkElement;
        //    cartPage = FindName("Page_Cart") as FrameworkElement;
        //    profilePage = FindName("Page_Profile") as FrameworkElement;
        }

        void loadMenuPage()
        {
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
            /*
            var stack = cartPage.FindName("Cart_SP_Orders") as StackPanel;
            stack.Children.Clear();

            curOrder = allOrders.First();
            var poDtos = allPizzaOrders.FindAll(e => e.order_id == curOrder.id);

            allPizzaOrders.Clear();

            foreach (var poDto in poDtos)
                addPizzaOrder(poDto);
            */
        }
        /*
        void updatePizzaOrderPrice(DTO.Pizza_Order poDto)
        {
            var pizzaDto = allPizzas.First(e => e.id == poDto.pizza_id);
            var sizeDto = allPizzaSizes.First(e => e.id == poDto.size_id);

            poDto.cost = pizzaDto.cost * sizeDto.cost_mult * poDto.quantity;
        }

        void updateOrderPrice()
        {
            decimal total = 0.0m;

            //      foreach (var poDto in allPizzaOrders.FindAll(e => e.order_id == curOrder.id))
            //         total += poDto.cost;

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
        */
        /*
        private Phone selectedPhone;

        public ObservableCollection<Phone> Phones { get; set; }
        public Phone SelectedPhone
        {
            get { return selectedPhone; }
            set
            {
                selectedPhone = value;
                OnPropertyChanged("SelectedPhone");
            }
        }

        // команда добавления нового объекта
        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      Phone phone = new Phone();
                      Phones.Insert(0, phone);
                      SelectedPhone = phone;
                  }));
            }
        }
        // команда удаления
        private RelayCommand removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                  (removeCommand = new RelayCommand(obj =>
                  {
                      Phone phone = obj as Phone;
                      if (phone != null)
                      {
                          Phones.Remove(phone);
                      }
                  },
                 //условие, при котором будет доступна команда
                 (obj) => (Phones.Count > 0 && selectedPhone != null)));
            }
        }
        private RelayCommand copyCommand;
        public RelayCommand CopyCommand
        {
            get
            {
                return copyCommand ??
                    (copyCommand = new RelayCommand(obj =>
                    {
                        Phone phone = obj as Phone;
                        if (phone != null)
                        {
                            Phone phoneCopy = new Phone
                            {
                                Company = phone.Company,
                                Price = phone.Price,
                                Title = phone.Title
                            };
                            Phones.Insert(0, phoneCopy);
                        }
                    }));
            }
        }



        public ApplicationViewModel(IDialogService dialogService, IFileService fileService)
        {
            //Для работы с файлами в конструктор ApplicationViewModel передаются объекты IDialogService и IFileService
            this.dialogService = dialogService;
            this.fileService = fileService;

            Phones = new ObservableCollection<Phone>
            {
                new Phone {Title="iPhone 7", Company="Apple", Price=56000, Models = new ObservableCollection<string>(new List<string>{ "iPhone 7 OLD", "iPhone 7 VERY OLD"}) },
                new Phone {Title="Galaxy S7 Edge", Company="Samsung", Price =60000 , Models = new ObservableCollection<string>(new List<string>{ "GaBolaxy XL", "Galaxy XXL"})},
                new Phone {Title="Xperia", Company="Sony", Price=56000 , Models = new ObservableCollection<string>(new List<string>{ "Girl", "Boys"})},
                new Phone {Title="Mi5S", Company="Xiaomi", Price=35000 , Models = new ObservableCollection<string>(new List<string>{ "Mimi","Mumu"})}
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        IFileService fileService;
        IDialogService dialogService;

        // команда сохранения файла
        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                  (saveCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          if (dialogService.SaveFileDialog() == true)
                          {
                              fileService.Save(dialogService.FilePath, Phones.ToList());
                              dialogService.ShowMessage("Файл сохранен");
                          }
                      }
                      catch (Exception ex)
                      {
                          dialogService.ShowMessage(ex.Message);
                      }
                  }));
            }
        }

        // команда открытия файла
        private RelayCommand openCommand;
        public RelayCommand OpenCommand
        {
            get
            {
                return openCommand ??
                  (openCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          if (dialogService.OpenFileDialog() == true)
                          {
                              var phones = fileService.Open(dialogService.FilePath);
                              Phones.Clear();
                              foreach (var p in phones)
                                  Phones.Add(p);
                              dialogService.ShowMessage("Файл открыт");
                          }
                      }
                      catch (Exception ex)
                      {
                          dialogService.ShowMessage(ex.Message);
                      }
                  }));
            }
        }*/
    }
}
