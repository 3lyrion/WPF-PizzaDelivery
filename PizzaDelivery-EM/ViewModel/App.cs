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

        List<DTO.Dough> allDough;
        List<DTO.Order> allOrders;
        List<DTO.Pizza> allPizzas;
        List<DTO.PizzaOrder> allPizzaOrders;
        List<DTO.PizzaSize> allPizzaSizes;

        public DTO.OrderStatus OriginalOrderStatus { get; set; }

        public ObservableCollection<Model.Pizza> Pizzas { get; set; }
        public ObservableCollection<Model.Order> PastOrders { get; set; }

        public List<string> OrderStatuses { get; set; }

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
                            CurrentOrder.Status = Misc.StringToOrderStatus((string)obj);
                        

                    }));
            }
        }

        public App(
            SV.ICook theCookService,
            SV.ICourier theCourierService,
            SV.IDough theDoughService,
            SV.IOrder theOrderService,
            SV.IPizza thePizzaService,
            SV.IPizzaOrder thePizzaOrderService,
            SV.IPizzaSize thePizzaSizeService
        )
        {
            cookService = theCookService;
            courierService = theCourierService;
            doughService = theDoughService;
            orderService = theOrderService;
            pizzaService = thePizzaService;
            pizzaOrderService = thePizzaOrderService;
            pizzaSizeService = thePizzaSizeService;

            load();
        }

        void gotoProfileMenu()
        {
            DTO.Cook cook = null;
            DTO.Courier courier = null;

            if (Account is DTO.Cook) cook = Account as DTO.Cook;
            else courier = Account as DTO.Courier;

            IEnumerable<Model.Order> orders = null;

            if (cook != null)
            {
                CurrentOrder = allOrders.First(e => e.CookId == Account.Id);

                orders = allOrders
                    .Where(e => e.CookId == Account.Id)
                    .OrderByDescending(e => e.CreationDate)
                    .Select(o => new Model.Order
                    {
                        Address = o.Address,
                        Cost = o.Cost.Value,
                        CreationDate = o.CreationDate,
                        Status = o.Status,
                        Parts = allPizzaOrders.Where(po => po.OrderId == o.Id).Select(p => new Model.OrderPart
                        {
                            //Cost = p.Cost.Value,
                            Dough = allDough.Find(a => a.Id == p.DoughId),
                            Pizza = Pizzas.First(a => a.Name == allPizzas.Find(b => b.Id == p.PizzaId).Name),
                            PizzaSize = allPizzaSizes.Find(a => a.Id == p.SizeId),
                            Quantity = p.Quantity

                        }).ToList()
                    });
            }
            
            else
            {
                orders = allOrders
                    .Where(e => e.CourierId == Account.Id)
                    .OrderByDescending(e => e.CreationDate)
                    .Select(o => new Model.Order
                    {
                        Address = o.Address,
                        Cost = o.Cost.Value,
                        CreationDate = o.CreationDate,
                        Status = o.Status,
                        Parts = allPizzaOrders.Where(po => po.OrderId == o.Id).Select(p => new Model.OrderPart
                        {
                            //Cost = p.Cost.Value,
                            Dough = allDough.Find(a => a.Id == p.DoughId),
                            Pizza = Pizzas.First(a => a.Name == allPizzas.Find(b => b.Id == p.PizzaId).Name),
                            PizzaSize = allPizzaSizes.Find(a => a.Id == p.SizeId),
                            Quantity = p.Quantity

                        }).ToList()
                    });
            }

            foreach (var order in orders)
                PastOrders.Add(order);

            OriginalOrderStatus = CurrentOrder.Status;

            ProfileMenuVisible = true;
        }

        void load()
        {
            loadMembers();
        }

        void loadMembers()
        {
            /*
            updateTimer = new Timer(5000);
            updateTimer.AutoReset = true;
            updateTimer.Elapsed += (s, e) =>
            {
                
            };
            updateTimer.Start();
            */

            OrderStatuses = new List<string>
            {
                "Готовится",
                "Доставляется",
                "Доставлен",
                "Отменён"
            };

            allDough = doughService.GetList();
            allOrders = orderService.GetList();
            allPizzaOrders = pizzaOrderService.GetList();
            allPizzas = pizzaService.GetList();
            allPizzaSizes = pizzaSizeService.GetList();


            PastOrders = new ObservableCollection<Model.Order>();
            /*
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
            */
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

            Account = cookService.GetList().First();
            gotoProfileMenu();
        }
    }
}
