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
    public partial class App : INotifyPropertyChanged
    {
        RelayCommand updateCustomPizzaCostCommand;
        public RelayCommand UpdateCustomPizzaCostCommand
        {
            get
            {
                return updateCustomPizzaCostCommand ??
                    (updateCustomPizzaCostCommand = new RelayCommand(obj =>
                    {
                        // Триггерю обновление цены
                        SelectedOrderPart.Quantity = SelectedOrderPart.Quantity;

                    }));
            }
        }

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
                            var clients = clientService.GetList();
                            var client = clients.Where(e => e.PhoneNumber == phoneNumber);

                            if (client.Count() == 0)
                                AuthErrorMessage = "Пользователя c таким номером не существует";

                            else if (password != client.First().Password)
                                AuthErrorMessage = "Неверный пароль";

                            else
                            {
                                Account = client.First();

                                gotoProfileMenu();
                            }
                        }
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

                        var phoneNumberTB = objects[0] as TextBox;
                        var passwordTB = objects[1] as PasswordBox;
                        var passwordConfirmationTB = objects[2] as PasswordBox;

                        var phoneNumber = phoneNumberTB.Text;
                        var password = passwordTB.Password;
                        var passwordConfirmation = passwordConfirmationTB.Password;

                        if (phoneNumber.Length != phoneNumberTB.MaxLength)
                            AuthErrorMessage = "Неверный номер телефона";

                        else if (password.Length < 6)
                            AuthErrorMessage = "Слишкой короткий пароль";

                        else if (passwordConfirmation != password)
                            AuthErrorMessage = "Пароли не совпадают";

                        else
                        {
                            var clients = clientService.GetList();

                            if (clients.Count(e => e.PhoneNumber == phoneNumber) != 0)
                                AuthErrorMessage = "Пользователь с таким номером уже существует";

                            else
                            {
                                Account = new DTO.Client
                                {
                                    Online = true,
                                    Password = password,
                                    PhoneNumber = phoneNumber,
                                    OrdersIDs = new List<int>()
                                };

                                Account.Id = clientService.Create(Account);

                                gotoProfileMenu();
                            }
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

                        // Имитация платежа
                        if (!PaymentSystem.Pay()) return;

                        var orderDto = new DTO.Order
                        {
                            Address = CurrentOrder.Address,
                            RecipientName = CurrentOrder.RecipientName,
                            Cost = CurrentOrder.Cost,
                            ClientId = Account.Id,
                            PizzaOrdersIds = new List<int>()
                        };

                        foreach (var op in OrderParts)
                        {
                            var pizzaOrderDto = new DTO.PizzaOrder
                            {
                                Cost = op.Cost,
                                Quantity = op.Quantity,
                                DoughId = allDough.Find(e => e.Name == op.Dough.Name).Id,
                                SizeId = allPizzaSizes.Find(e => e.Size == op.PizzaSize.Size).Id,
                            };

                            if (op.CustomId != 0)
                            {
                                allPizzas = pizzaService.GetList();

                                pizzaOrderDto.PizzaId = allPizzas.Find(e => e.Id == op.CustomId).Id;
                            }

                            else
                                pizzaOrderDto.PizzaId = allPizzas.Find(e => e.Name == op.Pizza.Name).Id;

                            pizzaOrderDto.Id = pizzaOrderService.Create(pizzaOrderDto);

                            orderDto.PizzaOrdersIds.Add(pizzaOrderDto.Id);
                        }

                        orderDto.Id = orderService.Create(orderDto);

                        CurrentOrder.Clear();

                        OrderParts.Clear();

                        updateData();
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

                    }, (obj) => (OrderParts.Count > 0 && Account != null)));
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

        RelayCommand createCustomPizzaCommand;
        public RelayCommand CreateCustomPizzaCommand
        {
            get
            {
                return createCustomPizzaCommand ??
                    (createCustomPizzaCommand = new RelayCommand(obj =>
                    {
                        CustomPizzaEditorVisible = true;

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
                        var objects = obj as object[];

                        if (objects[0] is Model.Pizza)
                        {
                            var pizza = objects[0] as Model.Pizza;

                            SelectedOrderPart = createOrderPart();

                            SelectedOrderPart.Pizza = pizza;
                            SelectedOrderPart.Quantity = 1;
                        }

                        else if (objects[0] is Model.OrderPart)
                        {
                            var orderPart = objects[0] as Model.OrderPart;

                            orderPart.CopyTo(OriginalOrderPart);

                            // Quantity = 1 для расчёта цены за штуку
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
                        
                        var pizza = SelectedOrderPart.Pizza;

                        if (pizza.Name == "Своя")
                        {
                            var pizzaId = pizzaService.Create(new DTO.Pizza
                            {
                                Cost = pizza.Cost,
                                Custom = true,
                                Name = pizza.Name
                            });

                            if (pizzaId != 0)
                            {
                                foreach (var ingredient in SelectedOrderPart.Pizza.Ingredients)
                                {
                                    recipeService.Create(new DTO.Recipe
                                    {
                                        IngredientId = allIngredients.Find(e => e.Name == ingredient.Name).Id,
                                        PizzaId = pizzaId
                                    });
                                }
                            }

                            SelectedOrderPart.CustomId = pizzaId;
                        }
                        
                        // Сначала добавляю в список, а потом триггерю обновление цен
                        OrderParts.Add(SelectedOrderPart);

                        SelectedOrderPart.Quantity = 1;

                    Finally:

                        SelectedOrderPart = null;

                        var editor = obj as View.Elements.OrderPartEditor;
                        editor.Visibility = Visibility.Hidden;

                    }, (obj) => (SelectedOrderPart != null && SelectedOrderPart.PizzaSize != null && SelectedOrderPart.Dough != null
                    && (SelectedOrderPart.Pizza.Name != "Своя" || SelectedOrderPart.Pizza.Name == "Своя" && SelectedOrderPart.Pizza.Ingredients.Count(e => e.Selected) > 1))));
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
    }
}
