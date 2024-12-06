using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using DTO = Interfaces.DTO;
using SV = Interfaces.Service;

namespace PizzaDelivery.ViewModel
{
    public partial class App : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        Timer updateTimer;

        SV.IClient clientService;
        SV.IDough doughService;
        SV.IIngredient ingredientService;
        SV.IOrder orderService;
        SV.IPizza pizzaService;
        SV.IPizzaOrder pizzaOrderService;
        SV.IPizzaSize pizzaSizeService;
        SV.IRecipe recipeService;

        List<DTO.Dough> allDough;
        List<DTO.Ingredient> allIngredients;
        List<DTO.Order> allOrders;
        List<DTO.Pizza> allPizzas;
        List<DTO.PizzaOrder> allPizzaOrders;
        List<DTO.PizzaSize> allPizzaSizes;
        List<DTO.Recipe> allRecipes;

        bool customPizzaEditorVisible = false;
        public bool CustomPizzaEditorVisible
        {
            get { return customPizzaEditorVisible; }
            set
            {
                customPizzaEditorVisible = value;
                OnPropertyChanged("CustomPizzaEditorVisible");
            }
        }

        bool profileMenuVisible = false;
        public bool ProfileMenuVisible
        {
            get { return profileMenuVisible; }
            set
            {
                profileMenuVisible = value;
                OnPropertyChanged("ProfileMenuVisible");
            }
        }

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

        string authErrorMessage;
        public string AuthErrorMessage
        {
            get { return authErrorMessage; }
            set
            {
                authErrorMessage = value;
                OnPropertyChanged("AuthErrorMessage");
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

        public ObservableCollection<Model.OrderPart> OrderParts { get; set; }

        List<Model.Order> __pastOrders;
        public List<Model.Order> PastOrders
        {
            get { return __pastOrders; }
            set
            {
                __pastOrders = value;
                OnPropertyChanged("PastOrders");
            }
        }

        List<Model.Order> __actualOrders;
        public List<Model.Order> ActualOrders
        {
            get { return __actualOrders; }
            set
            {
                __actualOrders = value;
                OnPropertyChanged("ActualOrders");
            }
        }

        List<Model.Dough> __dough;
        public List<Model.Dough> Dough
        {
            get { return __dough; }
            set
            {
                __dough = value;
                OnPropertyChanged("Dough");
            }
        }

        List<Model.Pizza> __pizzas;
        public List<Model.Pizza> Pizzas
        {
            get { return __pizzas; }
            set
            {
                __pizzas = value;
                OnPropertyChanged("Pizzas");
            }
        }

        List<Model.PizzaSize> __pizzaSizes;
        public List<Model.PizzaSize> PizzaSizes
        {
            get { return __pizzaSizes; }
            set
            {
                __pizzaSizes = value;
                OnPropertyChanged("PizzaSizes");
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

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (Account == null) return;

            Account.Online = false;
            clientService.Update(Account);
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

                    orderPart.Cost = (orderPart.Pizza.Cost + orderPart.Pizza.Ingredients.Where(e => e.Selected).Sum(e => e.Cost))
                        * orderPart.Quantity * (decimal)orderPart.PizzaSize.CostMult;

                    updateOrderCost();
                }
            };

            return orderPart;
        }

        void updateOrderCost()
        {
            CurrentOrder.Cost = OrderParts.Sum(e => e.Cost);
        }

        void gotoProfileMenu()
        {
            ProfileMenuVisible = true;

            var orders = allOrders
                .Where(e => e.ClientId == Account.Id)
                .OrderByDescending(e => e.CreationDate)
                .Select(o => new Model.Order
                {
                    Address = o.Address,
                    Cost = o.Cost,
                    CreationDate = o.CreationDate,
                    Status = o.Status,
                    Parts = allPizzaOrders.Where(po => po.OrderId == o.Id).Select(p => new Model.OrderPart
                    {
                        Cost = p.Cost,
                        Dough = allDough.Find(a => a.Id == p.DoughId),
                        Pizza = Pizzas.First(a => a.Name == allPizzas.Find(b => b.Id == p.PizzaId).Name),
                        PizzaSize = allPizzaSizes.Find(a => a.Id == p.SizeId),
                        Quantity = p.Quantity

                    }).ToList()
                }).ToList();

            var pastOrders = new List<Model.Order>();
            var actualOrders = new List<Model.Order>();
            foreach (var order in orders)
            {
                if (order.Status == DTO.OrderStatus.Success ||
                    order.Status == DTO.OrderStatus.Cancellation)
                {
                    pastOrders.Add(order);
                }

                else actualOrders.Add(order);
            }
            PastOrders = pastOrders;
            ActualOrders = actualOrders;
        }

        void load()
        {
            init();
        }

        void init()
        {
            updateTimer = new Timer(5000);
            updateTimer.Elapsed += (s, e) => updateData();
            updateTimer.AutoReset = true;

            allDough = doughService.GetList();
            allIngredients = ingredientService.GetList();
            allOrders = orderService.GetList();
            allPizzaOrders = pizzaOrderService.GetList();
            allPizzas = pizzaService.GetList();
            allPizzaSizes = pizzaSizeService.GetList();
            allRecipes = recipeService.GetList();

            OriginalOrderPart = new Model.OrderPart();

            CurrentOrder = new Model.Order();

            Dough = allDough.Select(e => new Model.Dough
            {
                Name = e.Name

            }).ToList();

            PizzaSizes = allPizzaSizes.Select(e => new Model.PizzaSize
            {
                CostMult = e.CostMult,
                Name = e.Name,
                Size = e.Size

            }).ToList();

            var pizzas = new List<Model.Pizza>();
            foreach (var pizzaDto in allPizzas)
            {
                if (pizzaDto.Custom) continue;

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

                pizzas.Add(pizza);
            }

            // Кастомная пицца
            var customPizza = new Model.Pizza
            {
                Cost = 289.0m,

                Ingredients = allIngredients
                .Select(e => new Model.Ingredient
                {
                    Name = e.Name,
                    Cost = e.Cost,
                    Quantity = 0
                })
                .ToList(),

                Name = "Своя"
            };
            
            pizzas.Add(customPizza);

            Pizzas = pizzas;

            OrderParts = new ObservableCollection<Model.OrderPart>();
        }

        void updateData()
        {
            if (Account == null) return;

            allOrders = orderService.GetList();

            var orders = allOrders
                .Where(e => e.ClientId == Account.Id)
                .OrderByDescending(e => e.CreationDate)
                .Select(o => new Model.Order
                {
                    Address = o.Address,
                    Cost = o.Cost,
                    CreationDate = o.CreationDate,
                    Status = o.Status,
                    Parts = allPizzaOrders.Where(po => po.OrderId == o.Id).Select(p => new Model.OrderPart
                    {
                        Cost = p.Cost,
                        Dough = allDough.Find(a => a.Id == p.DoughId),
                        Pizza = Pizzas.First(a => a.Name == allPizzas.Find(b => b.Id == p.PizzaId).Name),
                        PizzaSize = allPizzaSizes.Find(a => a.Id == p.SizeId),
                        Quantity = p.Quantity

                    }).ToList()
                }).ToList();

            var pastOrders = new List<Model.Order>();
            var actualOrders = new List<Model.Order>();
            foreach (var order in orders)
            {
                if (order.Status == DTO.OrderStatus.Success ||
                    order.Status == DTO.OrderStatus.Cancellation)
                {
                    pastOrders.Add(order);
                }

                else actualOrders.Add(order);
            }
            PastOrders = pastOrders;
            ActualOrders = actualOrders;
        }
    }
}
