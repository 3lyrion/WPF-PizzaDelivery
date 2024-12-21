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

        RelayCommand showIngredientsMenuCommand;
        public RelayCommand ShowIngredientsMenuCommand
        {
            get
            {
                return showIngredientsMenuCommand ??
                    (showIngredientsMenuCommand = new RelayCommand(obj =>
                    {
                        if (obj is Model.OrderPart)
                            SelectedIngredients = (obj as Model.OrderPart).Pizza.Ingredients;

                    }));
            }
        }

        RelayCommand closeIngredientsMenuCommand;
        public RelayCommand CloseIngredientsMenuCommand
        {
            get
            {
                return closeIngredientsMenuCommand ??
                    (closeIngredientsMenuCommand = new RelayCommand(obj =>
                    {
                        SelectedIngredients = null;

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
                        if (Account is DTO.Cook && SelectedOrderStatus == DTO.OrderStatus.Delivery)
                            orderService.PassToCourier(CurrentOrder.Id);

                        else
                            orderService.Close(CurrentOrder.Id, (int)SelectedOrderStatus, Account is DTO.Courier);

                        // Сброс и запуск таймера обновления данных
                        updateTimer.Interval = 2500;
                        updateTimer.Start();

                        CurrentOrder = null;

                    }, (obj) => (CurrentOrder != null && CurrentOrder.Status != SelectedOrderStatus)));
            }
        }
    }
}
