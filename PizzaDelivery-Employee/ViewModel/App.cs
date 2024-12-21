using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Timers;
using PD_Employee.Util;
using DTO = Interfaces.DTO;
using SV = Interfaces.Service;

namespace PD_Employee.ViewModel
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

        SV.ICourier courierService;
        SV.ICook cookService;
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

        public List<string> OrderStatuses { get; set; }

        public Model.Order OrderData { get; set; }

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

        List<Model.Order> pastOrders;
        public List<Model.Order> PastOrders
        {
            get { return pastOrders; }
            set
            {
                pastOrders = value;
                OnPropertyChanged("PastOrders");
            }
        }

        List<DTO.Ingredient> selectedIngredients;
        public List<DTO.Ingredient> SelectedIngredients
        {
            get { return selectedIngredients; }
            set
            {
                selectedIngredients = value;
                OnPropertyChanged("SelectedIngredients");
            }
        }

        DTO.OrderStatus selectedOrderStatus;
        public DTO.OrderStatus SelectedOrderStatus
        {
            get { return selectedOrderStatus; }
            set
            {
                selectedOrderStatus = value;
                OnPropertyChanged("SelectedOrderStatus");
            }
        }

        DTO.Order currentOrder;
        public DTO.Order CurrentOrder
        {
            get { return currentOrder; }
            set
            {
                currentOrder = value;
                OnPropertyChanged("CurrentOrder");
            }
        }

        DTO.Base.Employee account;
        public DTO.Base.Employee Account
        {
            get { return account; }
            set
            {
                account = value;
                OnPropertyChanged("Account");
            }
        }

        public App(
            SV.ICook theCookService,
            SV.ICourier theCourierService,
            SV.IDough theDoughService,
            SV.IIngredient theIngredientService,
            SV.IOrder theOrderService,
            SV.IPizza thePizzaService,
            SV.IPizzaOrder thePizzaOrderService,
            SV.IPizzaSize thePizzaSizeService,
            SV.IRecipe theRecipeService
        )
        {
            cookService = theCookService;
            courierService = theCourierService;
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

            if (Account is DTO.Cook) cookService.Update(Account as DTO.Cook);
            else courierService.Update(Account as DTO.Courier);
        }

        void gotoProfileMenu()
        {
            ProfileMenuVisible = true;

            DTO.Cook cook = null;
            DTO.Courier courier = null;

            if (Account is DTO.Cook) cook = Account as DTO.Cook;
            else courier = Account as DTO.Courier;

            IEnumerable<DTO.Order> filtered = null;
            IEnumerable<Model.Order> orders = null;

            if (cook != null)
            {
                cookService.Update(cook);

                filtered = allOrders.Where(e => e.CookId == Account.Id);
                try
                {
                    CurrentOrder = filtered.First(e => e.Status == DTO.OrderStatus.Preparation);
                }
                catch { }
            }

            else
            {
                courierService.Update(courier);

                filtered = allOrders.Where(e => e.CourierId == Account.Id);
                try
                {
                    CurrentOrder = filtered.First(e => e.Status == DTO.OrderStatus.Delivery);
                }
                catch { }
            }

            if (CurrentOrder != null)
            {
                SelectedOrderStatus = CurrentOrder.Status;

                OrderData.Address = CurrentOrder.Address;
                OrderData.RecipientName = CurrentOrder.RecipientName;
                OrderData.CreationDate = CurrentOrder.CreationDate;
                OrderData.Cost = CurrentOrder.Cost;

                var parts = new List<Model.OrderPart>();
                var pizzaOrders = allPizzaOrders.FindAll(e => e.OrderId == CurrentOrder.Id);
                foreach (var poDto in pizzaOrders)
                    parts.Add(new Model.OrderPart
                    {
                        Cost = poDto.Cost,
                        Dough = allDough.Find(a => a.Id == poDto.DoughId),
                        Pizza = Pizzas.First(a => a.Name == allPizzas.Find(b => b.Id == poDto.PizzaId).Name),
                        PizzaSize = allPizzaSizes.Find(a => a.Id == poDto.SizeId),
                        Quantity = poDto.Quantity
                    });

                OrderData.Parts = parts;
            }

            else Account.Busy = false;

            orders = filtered
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
                    });

            var pastOrders = orders.Where(e => e.Status == DTO.OrderStatus.Cancellation
                || e.Status == DTO.OrderStatus.Success).ToList();
        }

        void load()
        {
            init();
        }

        void init()
        {
            updateTimer = new Timer(2500);
            updateTimer.AutoReset = true;
            updateTimer.Elapsed += (s, e) => updateData();
            updateTimer.Start();

            OrderStatuses = new List<string>
            {
                "Доставляется",
                "Доставлен",
                "Отменён"
            };

            allDough = doughService.GetList();
            allIngredients = ingredientService.GetList();
            allOrders = orderService.GetList();
            allPizzaOrders = pizzaOrderService.GetList();
            allPizzas = pizzaService.GetList();
            allPizzaSizes = pizzaSizeService.GetList();
            allRecipes = recipeService.GetList();

            OrderData = new Model.Order
            {
                Parts = new List<Model.OrderPart>()
            };

            var pizzas = new List<Model.Pizza>();
            foreach (var pizzaDto in allPizzas)
            {
                var pizza = new Model.Pizza
                {
                    Name = pizzaDto.Name,
                    Cost = pizzaDto.Cost,
                    Ingredients = new List<DTO.Ingredient>()
                };

                var recipes = allRecipes.FindAll(e => e.PizzaId == pizzaDto.Id);
                
                foreach (var recipe in recipes)
                    pizza.Ingredients.Add(allIngredients.Find(e => e.Id == recipe.IngredientId));

                pizzas.Add(pizza);
            }
            Pizzas = pizzas;
        }

        void updateData()
        {
            if (Account == null) return;

            allOrders = orderService.GetList();
            allPizzaOrders = pizzaOrderService.GetList();
            allPizzas = pizzaService.GetList();
            allRecipes = recipeService.GetList();

        //    if (CurrentOrder != null) return;

            DTO.Cook cook = null;
            DTO.Courier courier = null;

            if (Account is DTO.Cook) cook = Account as DTO.Cook;
            else courier = Account as DTO.Courier;

            IEnumerable<DTO.Order> filtered = null;
            IEnumerable<Model.Order> orders = null;

            if (cook != null)
            {
                var cookDto = cookService.GetList().Find(e => e.Id == cook.Id);
                Account.Busy = cookDto.Busy;

                filtered = allOrders.Where(e => e.CookId == Account.Id);
                try
                {
                    CurrentOrder = filtered.First(e => e.Status == DTO.OrderStatus.Preparation);
                }
                catch { }
            }

            else
            {
                var courierDto = courierService.GetList().Find(e => e.Id == courier.Id);
                Account.Busy = courierDto.Busy;

                filtered = allOrders.Where(e => e.CourierId == Account.Id);
                try
                {
                    CurrentOrder = filtered.First(e => e.Status == DTO.OrderStatus.Delivery);
                }
                catch { }
            }

            if (CurrentOrder != null)
            {
                //    SelectedOrderStatus = CurrentOrder.Status;

                var pizzas = new List<Model.Pizza>();
                foreach (var pizzaDto in allPizzas)
                {
                    var pizza = new Model.Pizza
                    {
                        Name = pizzaDto.Name,
                        Cost = pizzaDto.Cost,
                        Ingredients = new List<DTO.Ingredient>()
                    };

                    var recipes = allRecipes.FindAll(e => e.PizzaId == pizzaDto.Id);

                    foreach (var recipe in recipes)
                        pizza.Ingredients.Add(allIngredients.Find(e => e.Id == recipe.IngredientId));

                    pizzas.Add(pizza);
                }
                Pizzas = pizzas;

                OrderData.Address = CurrentOrder.Address;
                OrderData.RecipientName = CurrentOrder.RecipientName;
                OrderData.CreationDate = CurrentOrder.CreationDate;
                OrderData.Cost = CurrentOrder.Cost;

                var parts = new List<Model.OrderPart>();
                var pizzaOrders = allPizzaOrders.FindAll(e => e.OrderId == CurrentOrder.Id);
                foreach (var poDto in pizzaOrders)
                    parts.Add(new Model.OrderPart
                    {
                        Cost = poDto.Cost,
                        Dough = allDough.Find(a => a.Id == poDto.DoughId),
                        Pizza = Pizzas.First(a => a.Name == allPizzas.Find(b => b.Id == poDto.PizzaId).Name),
                        PizzaSize = allPizzaSizes.Find(a => a.Id == poDto.SizeId),
                        Quantity = poDto.Quantity
                    });

                OrderData.Parts = parts;

                // Прекращение обновления данных
                updateTimer.Stop();
            }

            else Account.Busy = false;

            orders = filtered
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
                    });

            PastOrders = orders.Where(e => e.Status == DTO.OrderStatus.Cancellation || e.Status == DTO.OrderStatus.Success).ToList();
        }
    }
}
