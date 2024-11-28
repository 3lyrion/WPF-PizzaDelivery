using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using DTO = Interfaces.DTO;
using SV = Interfaces.Service;

namespace PizzaDelivery.ViewModel
{
    public class App : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        SV.IClient clientService;
        SV.IDough doughService;
        SV.IIngredient ingredientService;
        SV.IOrder orderService;
        SV.IPizza pizzaService;
        SV.IPizzaOrder pizzaOrderService;
        SV.IPizzaSize pizzaSizeService;
        SV.IRecipe recipeService;

        List<DTO.Client> allClients;
        List<DTO.Dough> allDough;
        List<DTO.Ingredient> allIngredients;
        List<DTO.Order> allOrders;
        List<DTO.Pizza> allPizzas;
        List<DTO.PizzaOrder> allPizzaOrders;
        List<DTO.PizzaSize> allPizzaSizes;
        List<DTO.Recipe> allRecipes;

        bool regMenuVisible = false;
        public bool RegistrationMenuVisible
        {
            get { return regMenuVisible; }
            set
            {
                regMenuVisible = value;
                OnPropertyChanged("RegistrationMenuVisible");
            }
        }

        bool checkoutMenuVisible = false;
        public bool CheckoutMenuVisible
        {
            get { return checkoutMenuVisible; }
            set
            {
                checkoutMenuVisible = value;
                OnPropertyChanged("CheckoutMenuVisible");
            }
        }

        Model.OrderPart selectedOrderPart;
        public Model.OrderPart SelectedOrderPart
        {
            get { return selectedOrderPart; }
            set
            {
                selectedOrderPart = value;
                OnPropertyChanged("SelectedOrderPart");
            }
        }

        DTO.Client account;
        public DTO.Client Account
        {
            get { return account; }
            set
            {
                account = value;
                OnPropertyChanged("Account");
            }
        }

        public Model.OrderPart OriginalOrderPart { get; set; }

        public Model.Order CurrentOrder { get; set; }

        public ObservableCollection<Model.Dough> Dough { get; set; }
        public ObservableCollection<Model.Ingredient> Ingredients { get; set; }
        public ObservableCollection<Model.Pizza> Pizzas { get; set; }
        public ObservableCollection<Model.PizzaSize> PizzaSizes { get; set; }
        public ObservableCollection<Model.OrderPart> OrderParts { get; set; }
        public ObservableCollection<Model.Order> PastOrders { get; set; }
        public ObservableCollection<Model.Order> ActualOrders { get; set; }

        RelayCommand gotoLoginMenuCommand;
        public RelayCommand GoToLoginMenuCommand
        {
            get
            {
                return gotoLoginMenuCommand ??
                    (gotoLoginMenuCommand = new RelayCommand(obj =>
                    {
                        RegistrationMenuVisible = false;

                    }));
            }
        }

        RelayCommand gotoRegMenuCommand;
        public RelayCommand GoToRegistrationMenuCommand
        {
            get
            {
                return gotoRegMenuCommand ??
                    (gotoRegMenuCommand = new RelayCommand(obj =>
                    {
                        RegistrationMenuVisible = true;

                    }));
            }
        }

        RelayCommand loginCommand;
        public RelayCommand LoginCommand
        {
            get
            {
                return loginCommand ??
                    (loginCommand = new RelayCommand(obj =>
                    {
                        var objects = obj as object[];
                        var phoneNumber = (objects[0] as TextBox).Text;
                        var password = (objects[1] as PasswordBox).Password;

                        try
                        {
                            Account = allClients.Find(e => e.PhoneNumber == phoneNumber && e.Password == password);
                            loadOrders();
                        }

                        catch { }

                    }));
            }
        }

        RelayCommand regCommand;
        public RelayCommand RegistrationCommand
        {
            get
            {
                return regCommand ??
                    (regCommand = new RelayCommand(obj =>
                    {
                        var objects = obj as object[];
                        var phoneNumber = (objects[0] as TextBox).Text;
                        var password = (objects[1] as PasswordBox).Password;
                        var passwordConfirmation = (objects[2] as PasswordBox).Password;

                        if (phoneNumber != "" &&
                            password != "" && passwordConfirmation == password)
                        {
                            Account = new DTO.Client
                            {
                                Online = true,
                                Password = password,
                                PhoneNumber = phoneNumber
                            };

                            loadOrders();
                        }

                    }));
            }
        }

        RelayCommand payOrderCommand;
        public RelayCommand PayOrderCommand
        {
            get
            {
                return payOrderCommand ??
                    (payOrderCommand = new RelayCommand(obj =>
                    {
                        CheckoutMenuVisible = false;
                        
                        allOrders.Add(new DTO.Order
                        {
                            Address = CurrentOrder.Address,
                            Cost = CurrentOrder.Cost
                        });

                        foreach (var op in OrderParts)
                        {
                            allPizzaOrders.Add(new DTO.PizzaOrder
                            {
                                Cost = op.Cost,
                                DoughId = allDough.Find(e => e.Name == op.Dough.Name).Id,
                                PizzaId = allPizzas.Find(e => e.Name == op.Pizza.Name).Id,
                                SizeId = allPizzaSizes.Find(e => e.Size == op.PizzaSize.Size).Id
                            });
                        }

                        CurrentOrder.Clear();

                        OrderParts.Clear();

                    }));
            }
        }

        RelayCommand gotoCheckoutMenuCommand;
        public RelayCommand GoToCheckoutMenuCommand
        {
            get
            {
                return gotoCheckoutMenuCommand ??
                    (gotoCheckoutMenuCommand = new RelayCommand(obj =>
                    {
                       CheckoutMenuVisible = true;

                    }, (obj) => (OrderParts.Count > 0/* && Account != null*/)));
            }
        }

        RelayCommand closeCheckoutMenuCommand;
        public RelayCommand CloseCheckoutMenuCommand
        {
            get
            {
                return closeCheckoutMenuCommand ??
                    (closeCheckoutMenuCommand = new RelayCommand(obj =>
                    {
                        CheckoutMenuVisible = false;

                    }));
            }
        }

        RelayCommand selectPizzaSizeCommand;
        public RelayCommand SelectPizzaSizeCommand
        {
            get
            {
                return selectPizzaSizeCommand ??
                    (selectPizzaSizeCommand = new RelayCommand(obj =>
                    {
                        if (obj is Model.PizzaSize)
                        {
                            var pizzaSize = obj as Model.PizzaSize;

                            SelectedOrderPart.PizzaSize = allPizzaSizes.Find(e => e.Size == pizzaSize.Size);
                        }
                    }));
            }
        }

        RelayCommand selectDoughCommand;
        public RelayCommand SelectDoughCommand
        {
            get
            {
                return selectDoughCommand ??
                    (selectDoughCommand = new RelayCommand(obj =>
                    {
                        if (obj is Model.Dough)
                        {
                            var dough = obj as Model.Dough;

                            SelectedOrderPart.Dough = allDough.Find(e => e.Name == dough.Name);
                        }
                    }));
            }
        }

        RelayCommand editOrderPartCommand;
        public RelayCommand EditOrderPartCommand
        {
            get
            {
                return editOrderPartCommand ??
                    (editOrderPartCommand = new RelayCommand(obj =>
                    {
                        // Quantity = 1 для правильного расчёта цены

                        var objects = obj as object[];

                        if (objects[0] is Model.Pizza)
                        {
                            var pizza = objects[0] as Model.Pizza;

                            SelectedOrderPart = createOrderPart();

                            SelectedOrderPart.CopyTo(OriginalOrderPart);

                            SelectedOrderPart.Pizza = pizza;
                            SelectedOrderPart.Quantity = 1;
                        }

                        else if (objects[0] is Model.OrderPart)
                        {
                            var orderPart = objects[0] as Model.OrderPart;

                            orderPart.CopyTo(OriginalOrderPart);

                            orderPart.Quantity = 1;

                            SelectedOrderPart = orderPart;
                        }

                        var editor = objects[1] as View.Elements.OrderPartEditor;
                        editor.Visibility = Visibility.Visible;

                    }));
            }
        }

        RelayCommand cancelOrderPartCommand;
        public RelayCommand CancelOrderPartCommand
        {
            get
            {
                return cancelOrderPartCommand ??
                  (cancelOrderPartCommand = new RelayCommand(obj =>
                  {
                      var editor = obj as View.Elements.OrderPartEditor;

                      OriginalOrderPart.CopyTo(SelectedOrderPart);
                      SelectedOrderPart = null;

                      editor.Visibility = Visibility.Hidden;

                  }));
            }
        }

        RelayCommand submitOrderPartCommand;
        public RelayCommand SubmitOrderPartCommand
        {
            get
            {
                return submitOrderPartCommand ??
                    (submitOrderPartCommand = new RelayCommand(obj =>
                    {
                        var editor = obj as View.Elements.OrderPartEditor;

                        // Изменение существующей части заказа
                        foreach (var orderPart in OrderParts)
                        {
                            if (orderPart == SelectedOrderPart)
                            {
                                orderPart.Quantity = OriginalOrderPart.Quantity;

                                goto Finally;
                            }

                            // Если уже есть в точности такая же пицца,
                            // то зачем добавлять новую часть заказа?
                            if (orderPart.Pizza == SelectedOrderPart.Pizza
                                && orderPart.Dough == SelectedOrderPart.Dough
                                && orderPart.PizzaSize == SelectedOrderPart.PizzaSize)
                            {
                                IncreasePizzaQuantityCommand.Execute(orderPart);

                                goto Finally;
                            }
                        }

                        // Сначала добавляю в список, а потом триггерю обновление цен
                        OrderParts.Add(SelectedOrderPart);

                        SelectedOrderPart.Quantity = 1;

                    Finally:

                        SelectedOrderPart = null;

                        editor.Visibility = Visibility.Hidden;

                    }, (obj) => (SelectedOrderPart != null && SelectedOrderPart.PizzaSize != null && SelectedOrderPart.Dough != null)));
            }
        }

        RelayCommand incrPizzaQuantityCommand;
        public RelayCommand IncreasePizzaQuantityCommand
        {
            get
            {
                return incrPizzaQuantityCommand ??
                  (incrPizzaQuantityCommand = new RelayCommand(obj =>
                  {
                      if (obj is Model.OrderPart)
                      {
                          var orderPart = obj as Model.OrderPart;

                          orderPart.Quantity++;
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
                      if (obj is Model.OrderPart)
                      {
                          var orderPart = obj as Model.OrderPart;

                          if (orderPart.Quantity > 0)
                              orderPart.Quantity--;

                          if (orderPart.Quantity == 0)
                              OrderParts.Remove(orderPart);
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
                          var orderPart = obj as Model.OrderPart;

                          orderPart.Quantity = 0;

                          OrderParts.Remove(orderPart);
                      }
                  }));
            }
        }

        public App(
            SV.IClient theClientService,
            SV.IDough theDoughService,
            SV.IIngredient theIngredientService,
            SV.IOrder theOrderService,
            SV.IPizza thePizzaService,
            SV.IPizzaOrder thePizzaOrderService,
            SV.IPizzaSize thePizzaSizeService,
            SV.IRecipe theRecipeService
        )
        {
            clientService = theClientService;
            doughService = theDoughService;
            ingredientService = theIngredientService;
            orderService = theOrderService;
            pizzaService = thePizzaService;
            pizzaOrderService = thePizzaOrderService;
            pizzaSizeService = thePizzaSizeService;
            recipeService = theRecipeService;

            load();
        }

        Model.OrderPart createOrderPart()
        {
            var orderPart = new Model.OrderPart();
            orderPart.Dough = allDough.Find(e => e.Name == "Традиционное");
            orderPart.PizzaSize = allPizzaSizes.Find(e => e.Size == 25);
            orderPart.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "PizzaSize" || e.PropertyName == "Quantity")
                {
                    if (orderPart.Pizza == null) return;

                    orderPart.Cost = orderPart.Pizza.Cost * orderPart.Quantity * (decimal)orderPart.PizzaSize.CostMult;

                    updateOrderCost();
                }
            };

            return orderPart;
        }

        void updateOrderCost()
        {
            CurrentOrder.Cost = OrderParts.Sum(e => e.Cost);
        }

        void loadOrders()
        {
            var orders = allOrders
                .Where(e => e.ClientId == Account.Id)
                .Select(o => new Model.Order
                {
                    Address = o.Address,
                    Cost = o.Cost.Value,
                    CreationTime = o.CreationDate,
                    Status = o.Status,
                    Parts = allPizzaOrders.Where(po => po.OrderId == o.Id).Select(p => new Model.OrderPart
                    {
                        //Cost = p.Cost.Value,
                        Dough = allDough.Find(a => a.Id == p.DoughId),
                        Pizza = Pizzas.First(a => a.Name == allPizzas.Find(b => b.Id == p.PizzaId).Name),
                        PizzaSize = allPizzaSizes.Find(a => a.Id == p.SizeId),
                        Quantity = p.Quantity

                    }).ToList()
                }).ToList();

            foreach (var order in orders)
            {
                PastOrders.Add(order);

                /*
                if (order.Status == DTO.OrderStatus.Success ||
                    order.Status == DTO.OrderStatus.Cancellation)
                {
                    PastOrders.Add(order);
                }

                else ActualOrders.Add(order);
                */
            }
        }

        void load()
        {
            loadMembers();
        }

        void loadMembers()
        {
            allClients = clientService.GetList();
            allDough = doughService.GetList();
            allIngredients = ingredientService.GetList();
            allOrders = orderService.GetList();
            allPizzaOrders = pizzaOrderService.GetList();
            allPizzas = pizzaService.GetList();
            allPizzaSizes = pizzaSizeService.GetList();
            allRecipes = recipeService.GetList();

            OriginalOrderPart = new Model.OrderPart();

            CurrentOrder = new Model.Order();

            ActualOrders = new ObservableCollection<Model.Order>();
            PastOrders = new ObservableCollection<Model.Order>();

            Dough = new ObservableCollection<Model.Dough>();
            foreach (var doughDto in allDough)
                Dough.Add(new Model.Dough
                {
                    Name = doughDto.Name
                });

            PizzaSizes = new ObservableCollection<Model.PizzaSize>();
            foreach (var sizeDto in allPizzaSizes)
                PizzaSizes.Add(new Model.PizzaSize
                {
                    CostMult = sizeDto.CostMult,
                    Name = sizeDto.Name,
                    Size = sizeDto.Size
                });

            Pizzas = new ObservableCollection<Model.Pizza>();
            foreach (var pizzaDto in allPizzas)
            {
                var pizza = new Model.Pizza
                {
                    Name = pizzaDto.Name,
                    Cost = pizzaDto.Cost
                };

                var recipes = allRecipes.FindAll(e => e.PizzaId == pizzaDto.Id);
                var ingredients = new List<Tuple<int, DTO.Ingredient>>();
                foreach (var recipe in recipes)
                    ingredients.Add(new Tuple<int, DTO.Ingredient>(recipe.Quantity, allIngredients.Find(e => e.Id == recipe.IngredientId)));

                pizza.Ingredients = ingredients.Select(e => new Model.Ingredient
                {
                    Name = e.Item2.Name,
                    Cost = e.Item2.Cost,
                    InStock = e.Item2.InStock,
                    Quantity = e.Item1
                })
                .ToList();

                Pizzas.Add(pizza);
            }
            
            OrderParts = new ObservableCollection<Model.OrderPart>();
        }
    }
}
