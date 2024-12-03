using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Timers;
using PizzaDelivery_EM.Util;
using DTO = Interfaces.DTO;
using SV = Interfaces.Service;

namespace PizzaDelivery_EM.ViewModel
{
    public class App : INotifyPropertyChanged
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
        SV.IOrder orderService;
        SV.IPizza pizzaService;
        SV.IPizzaOrder pizzaOrderService;
        SV.IPizzaSize pizzaSizeService;
        SV.ITransaction transactionService;

        List<DTO.Dough> allDough;
        List<DTO.Order> allOrders;
        List<DTO.Pizza> allPizzas;
        List<DTO.PizzaOrder> allPizzaOrders;
        List<DTO.PizzaSize> allPizzaSizes;

        public ObservableCollection<Model.Pizza> Pizzas { get; set; }
        public ObservableCollection<Model.Order> PastOrders { get; set; }

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

        RelayCommand loginCommand;
        public RelayCommand LoginCommand
        {
            get
            {
                return loginCommand ??
                    (loginCommand = new RelayCommand(obj =>
                    {
                        var objects = obj as object[];

                        var phoneNumberTB = objects[0] as TextBox;
                        var passwordTB = objects[1] as PasswordBox;

                        var phoneNumber = phoneNumberTB.Text;
                        var password = passwordTB.Password;

                        if (phoneNumber.Length != phoneNumberTB.MaxLength)
                            AuthErrorMessage = "Неверный номер телефона";

                        else if (password.Length < 6)
                            AuthErrorMessage = "Неверный пароль";

                        else
                        {
                            var cooks = cookService.GetList();
                            var couriers = courierService.GetList();
                            var cook = cooks.Where(e => e.PhoneNumber == phoneNumber);
                            var courier = couriers.Where(e => e.PhoneNumber == phoneNumber);

                            DTO.Base.Employee employee = null;

                            if (cook.Count() == 0 && courier.Count() == 0)
                                AuthErrorMessage = "Пользователя c таким номером не существует";

                            else
                            {
                                if (cook.Count() == 1) employee = cook.First();
                                else employee = courier.First();

                                if (password != employee.Password)
                                    AuthErrorMessage = "Неверный пароль";

                                else
                                {
                                    Account = employee;
                                    Account.Online = true;

                                    gotoProfileMenu();
                                }
                            }
                        }
                    }));
            }
        }

        RelayCommand selectOrderStatusCommand;
        public RelayCommand SelectOrderStatusCommand
        {
            get
            {
                return selectOrderStatusCommand ??
                    (selectOrderStatusCommand = new RelayCommand(obj =>
                    {
                        if (obj is string)
                            SelectedOrderStatus = Misc.StringToOrderStatus((string)obj);

                    }));
            }
        }

        RelayCommand changeOrderStatusCommand;
        public RelayCommand ChangeOrderStatusCommand
        {
            get
            {
                return changeOrderStatusCommand ??
                    (changeOrderStatusCommand = new RelayCommand(obj =>
                    {
                        CurrentOrder.Status = SelectedOrderStatus;

                        if (Account is DTO.Cook && CurrentOrder.Status == DTO.OrderStatus.Delivery)
                            transactionService.PassOrderToCourier(CurrentOrder.Id);

                        else
                            transactionService.CloseOrder(CurrentOrder.Id, (int)SelectedOrderStatus);

                        CurrentOrder = null;

                    }, (obj) => (CurrentOrder != null && CurrentOrder.Status != SelectedOrderStatus)));
            }
        }

        public App(
            SV.ICook theCookService,
            SV.ICourier theCourierService,
            SV.IDough theDoughService,
            SV.IOrder theOrderService,
            SV.IPizza thePizzaService,
            SV.IPizzaOrder thePizzaOrderService,
            SV.IPizzaSize thePizzaSizeService,
            SV.ITransaction theTransactionService
        )
        {
            cookService = theCookService;
            courierService = theCourierService;
            doughService = theDoughService;
            orderService = theOrderService;
            pizzaService = thePizzaService;
            pizzaOrderService = thePizzaOrderService;
            pizzaSizeService = thePizzaSizeService;
            transactionService = theTransactionService;

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
                filtered = allOrders.Where(e => e.CookId == Account.Id);
                try
                {
                    CurrentOrder = filtered.First(e => e.Status == DTO.OrderStatus.Preparation);
                    cook.Busy = true;
                }
                catch { }

                cookService.Update(cook);
            }

            else
            {
                filtered = allOrders.Where(e => e.CourierId == Account.Id);
                try
                {
                    CurrentOrder = filtered.First(e => e.Status == DTO.OrderStatus.Delivery);
                    courier.Busy = true;
                }
                catch { }

                courierService.Update(courier);
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

            foreach (var order in orders)
            if (order.Status == DTO.OrderStatus.Cancellation || order.Status == DTO.OrderStatus.Success)
                PastOrders.Add(order);
        }

        void load()
        {
            init();
        }

        void init()
        {
            updateTimer = new Timer(5000);
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
            allOrders = orderService.GetList();
            allPizzaOrders = pizzaOrderService.GetList();
            allPizzas = pizzaService.GetList();
            allPizzaSizes = pizzaSizeService.GetList();

            OrderData = new Model.Order
            {
                Parts = new List<Model.OrderPart>()
            };

            PastOrders = new ObservableCollection<Model.Order>();

            Pizzas = new ObservableCollection<Model.Pizza>();
            foreach (var pizzaDto in allPizzas)
            {
                var pizza = new Model.Pizza
                {
                    Name = pizzaDto.Name,
                    Cost = pizzaDto.Cost
                };

                Pizzas.Add(pizza);
            }

            // Кастомная пицца
            var customPizza = new Model.Pizza
            {
                Cost = 289.0m,
                Name = "Своя"
            };

            Pizzas.Add(customPizza);
        }

        void updateData()
        {
            if (Account == null) return;

            allOrders = orderService.GetList();
            allPizzaOrders = pizzaOrderService.GetList();

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
                    cook.Busy = true;
                }
                catch { }

                cookService.Update(cook);
            }

            else
            {
                var courierDto = courierService.GetList().Find(e => e.Id == courier.Id);
                Account.Busy = courierDto.Busy;

                filtered = allOrders.Where(e => e.CourierId == Account.Id);
                try
                {
                    CurrentOrder = filtered.First(e => e.Status == DTO.OrderStatus.Delivery);
                    courier.Busy = true;
                }
                catch { }

                courierService.Update(courier);
            }

            if (CurrentOrder != null)
            {
            //    SelectedOrderStatus = CurrentOrder.Status;

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

            if (PastOrders.Count > 0) PastOrders.Clear();
            foreach (var order in orders)
                if (order.Status == DTO.OrderStatus.Cancellation || order.Status == DTO.OrderStatus.Success)
                    PastOrders.Add(order);
        }
    }
}
