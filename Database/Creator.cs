using System;
using System.Linq;
using DM = DomainModel;

namespace Database
{
    public static class Creator
    {
        public static void CreateIfNotExists()
        {
            using (var db = new DAL.PizzaDeliveryDB())
            {
                if (db.Database.Exists()) return;

                // --------- Pizza_Size ---------
                {
                    Func<DM.Pizza_Size, DM.Pizza_Size> add = db.Pizza_Size.Add;

                    add(new DM.Pizza_Size
                    {
                        name = "Маленькая",
                        size = 25,
                        cost_mult = 1.00,
                        weight_mult = 1.00
                    });

                    add(new DM.Pizza_Size
                    {
                        name = "Средняя",
                        size = 30,
                        cost_mult = 1.30,
                        weight_mult = 1.20
                    });

                    add(new DM.Pizza_Size
                    {
                        name = "Большая",
                        size = 35,
                        cost_mult = 1.60,
                        weight_mult = 1.40
                    });

                    db.SaveChanges();
                }
                // Pizza_Size

                // --------- Dough ---------
                {
                    Func<DM.Dough, DM.Dough> add = db.Dough.Add;

                    add(new DM.Dough
                    {
                        name = "Традиционное",
                        weight = 100
                    });

                    add(new DM.Dough
                    {
                        name = "Тонкое",
                        weight = 60
                    });

                    db.SaveChanges();
                }
                // Dough

                // --------- Ingredient ---------
                {
                    Func<DM.Ingredient, DM.Ingredient> add = db.Ingredient.Add;

                    add(new DM.Ingredient
                    {
                        name = "Цыплёнок",
                        cost = 79.0m
                    });

                    add(new DM.Ingredient
                    {
                        name = "Соус песто",
                        cost = 20.0m
                    });

                    add(new DM.Ingredient
                    {
                        name = "Соус альфредо",
                        cost = 20.0m
                    });

                    add(new DM.Ingredient
                    {
                        name = "Брынза",
                        cost = 99.0m
                    });

                    add(new DM.Ingredient
                    {
                        name = "Моцарелла",
                        cost = 79.0m
                    });

                    add(new DM.Ingredient
                    {
                        name = "Томаты",
                        cost = 59.0m
                    });

                    add(new DM.Ingredient
                    {
                        name = "Ананасы",
                        cost = 59.0m
                    });

                    add(new DM.Ingredient
                    {
                        name = "Ветчина",
                        cost = 79.0m
                    });

                    db.SaveChanges();
                }
                // Ingredient

                // --------- Pizza ---------
                {
                    Func<DM.Pizza, DM.Pizza> add = db.Pizza.Add;

                    add(new DM.Pizza
                    {
                        name = "Песто",
                        cost = 499.00m,
                        weight = 400
                    });

                    add(new DM.Pizza
                    {
                        name = "Двойной цыплёнок",
                        cost = 389.00m,
                        weight = 360
                    });

                    add(new DM.Pizza
                    {
                        name = "Гавайская",
                        cost = 449.00m,
                        weight = 390
                    });

                    add(new DM.Pizza
                    {
                        name = "Ветчина и сыр",
                        cost = 389.00m,
                        weight = 320
                    });

                    db.SaveChanges();
                }
                // Pizza

                // --------- Recipe ---------
                {
                    Func<DM.Recipe, DM.Recipe> add = db.Recipe.Add;

                    var selPizza = db.Pizza.Single(e => e.name == "Песто");

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Цыплёнок"),
                        pizza = selPizza,
                        quantity = 130
                    });

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Соус песто"),
                        pizza = selPizza,
                        quantity = 15
                    });

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Брынза"),
                        pizza = selPizza,
                        quantity = 40
                    });

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Томаты"),
                        pizza = selPizza,
                        quantity = 60
                    });

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Моцарелла"),
                        pizza = selPizza,
                        quantity = 40
                    });

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Соус альфредо"),
                        pizza = selPizza,
                        quantity = 15
                    });

                    selPizza = db.Pizza.Single(e => e.name == "Двойной цыплёнок");

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Цыплёнок"),
                        pizza = selPizza,
                        quantity = 270
                    });

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Моцарелла"),
                        pizza = selPizza,
                        quantity = 60
                    });

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Соус альфредо"),
                        pizza = selPizza,
                        quantity = 30
                    });

                    selPizza = db.Pizza.Single(e => e.name == "Гавайская");

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Цыплёнок"),
                        pizza = selPizza,
                        quantity = 210
                    });

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Ананасы"),
                        pizza = selPizza,
                        quantity = 120
                    });

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Моцарелла"),
                        pizza = selPizza,
                        quantity = 40
                    });

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Соус альфредо"),
                        pizza = selPizza,
                        quantity = 20
                    });

                    selPizza = db.Pizza.Single(e => e.name == "Ветчина и сыр");

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Ветчина"),
                        pizza = selPizza,
                        quantity = 170
                    });

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Моцарелла"),
                        pizza = selPizza,
                        quantity = 120
                    });

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Соус альфредо"),
                        pizza = selPizza,
                        quantity = 30
                    });

                    db.SaveChanges();
                }
                // Recipe

                // --------- Courier ---------
                {
                    Func<DM.Courier, DM.Courier> add = db.Courier.Add;

                    add(new DM.Courier
                    {
                        full_name = "Смирнов Алексей Николаевич",
                        phone_number = "+79001234567",
                        password = "Qwerty1234"
                    });

                    db.SaveChanges();
                }
                // Courier

                // --------- Cook ---------
                {
                    Func<DM.Cook, DM.Cook> add = db.Cook.Add;

                    add(new DM.Cook
                    {
                        full_name = "Громова Светлана Валерьевна",
                        phone_number = "+79021234567",
                        password = "CookMaster88!"
                    });

                    db.SaveChanges();
                }
                // Cook
            }
        }

        public static void CreateMoke()
        {
            using (var db = new DAL.PizzaDeliveryDB())
            {
                if (db.Database.Exists()) return;

                // --------- Pizza_Size ---------
                {
                    Func<DM.Pizza_Size, DM.Pizza_Size> add = db.Pizza_Size.Add;

                    add(new DM.Pizza_Size
                    {
                        name = "Маленькая",
                        size = 25,
                        cost_mult = 1.00,
                        weight_mult = 1.00
                    });

                    add(new DM.Pizza_Size
                    {
                        name = "Средняя",
                        size = 30,
                        cost_mult = 1.30,
                        weight_mult = 1.20
                    });

                    add(new DM.Pizza_Size
                    {
                        name = "Большая",
                        size = 35,
                        cost_mult = 1.60,
                        weight_mult = 1.40
                    });

                    db.SaveChanges();
                }
                // Pizza_Size

                // --------- Dough ---------
                {
                    Func<DM.Dough, DM.Dough> add = db.Dough.Add;

                    add(new DM.Dough
                    {
                        name = "Традиционное",
                        weight = 100
                    });

                    add(new DM.Dough
                    {
                        name = "Тонкое",
                        weight = 60
                    });

                    db.SaveChanges();
                }
                // Dough

                // --------- Ingredient ---------
                {
                    Func<DM.Ingredient, DM.Ingredient> add = db.Ingredient.Add;

                    add(new DM.Ingredient
                    {
                        name = "Цыплёнок",
                        cost = 79.0m
                    });

                    add(new DM.Ingredient
                    {
                        name = "Соус песто",
                        cost = 20.0m
                    });

                    add(new DM.Ingredient
                    {
                        name = "Соус альфредо",
                        cost = 20.0m
                    });

                    add(new DM.Ingredient
                    {
                        name = "Брынза",
                        cost = 99.0m
                    });

                    add(new DM.Ingredient
                    {
                        name = "Моцарелла",
                        cost = 79.0m
                    });

                    add(new DM.Ingredient
                    {
                        name = "Томаты",
                        cost = 59.0m
                    });

                    add(new DM.Ingredient
                    {
                        name = "Ананасы",
                        cost = 59.0m
                    });

                    add(new DM.Ingredient
                    {
                        name = "Ветчина",
                        cost = 79.0m
                    });

                    db.SaveChanges();
                }
                // Ingredient

                // --------- Pizza ---------
                {
                    Func<DM.Pizza, DM.Pizza> add = db.Pizza.Add;

                    add(new DM.Pizza
                    {
                        name = "Песто",
                        cost = 499.00m,
                        weight = 400
                    });

                    add(new DM.Pizza
                    {
                        name = "Двойной цыплёнок",
                        cost = 389.00m,
                        weight = 360
                    });

                    add(new DM.Pizza
                    {
                        name = "Гавайская",
                        cost = 449.00m,
                        weight = 390
                    });

                    add(new DM.Pizza
                    {
                        name = "Ветчина и сыр",
                        cost = 389.00m,
                        weight = 320
                    });

                    db.SaveChanges();
                }
                // Pizza

                // --------- Recipe ---------
                {
                    Func<DM.Recipe, DM.Recipe> add = db.Recipe.Add;

                    var selPizza = db.Pizza.Single(e => e.name == "Песто");

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Цыплёнок"),
                        pizza = selPizza,
                        quantity = 130
                    });

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Соус песто"),
                        pizza = selPizza,
                        quantity = 15
                    });

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Брынза"),
                        pizza = selPizza,
                        quantity = 40
                    });

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Томаты"),
                        pizza = selPizza,
                        quantity = 60
                    });

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Моцарелла"),
                        pizza = selPizza,
                        quantity = 40
                    });

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Соус альфредо"),
                        pizza = selPizza,
                        quantity = 15
                    });

                    selPizza = db.Pizza.Single(e => e.name == "Двойной цыплёнок");

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Цыплёнок"),
                        pizza = selPizza,
                        quantity = 270
                    });

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Моцарелла"),
                        pizza = selPizza,
                        quantity = 60
                    });

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Соус альфредо"),
                        pizza = selPizza,
                        quantity = 30
                    });

                    selPizza = db.Pizza.Single(e => e.name == "Гавайская");

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Цыплёнок"),
                        pizza = selPizza,
                        quantity = 210
                    });

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Ананасы"),
                        pizza = selPizza,
                        quantity = 120
                    });

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Моцарелла"),
                        pizza = selPizza,
                        quantity = 40
                    });

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Соус альфредо"),
                        pizza = selPizza,
                        quantity = 20
                    });

                    selPizza = db.Pizza.Single(e => e.name == "Ветчина и сыр");

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Ветчина"),
                        pizza = selPizza,
                        quantity = 170
                    });

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Моцарелла"),
                        pizza = selPizza,
                        quantity = 120
                    });

                    add(new DM.Recipe
                    {
                        ingredient = db.Ingredient.Single(e => e.name == "Соус альфредо"),
                        pizza = selPizza,
                        quantity = 30
                    });

                    db.SaveChanges();
                }
                // Recipe

                // --------- Client ---------
                {
                    Func<DM.Client, DM.Client> add = db.Client.Add;

                    add(new DM.Client
                    {
                        phone_number = "+79123456789",
                        password = "qwerty123",
                        online = true
                    });

                    add(new DM.Client
                    {
                        phone_number = "+79112345678",
                        password = "password456"
                    });

                    add(new DM.Client
                    {
                        phone_number = "+79039876543",
                        password = "mysecret789",
                        online = true
                    });

                    add(new DM.Client
                    {
                        phone_number = "+79612345678",
                        password = "12345abc"
                    });

                    add(new DM.Client
                    {
                        phone_number = "+79056781234",
                        password = "letmein10",
                        online = true
                    });

                    add(new DM.Client
                    {
                        phone_number = "+79012345678",
                        password = "Magic2023!"
                    });

                    add(new DM.Client
                    {
                        phone_number = "+79013456789",
                        password = "Sunshine!456",
                        online = true
                    });

                    add(new DM.Client
                    {
                        phone_number = "+79014567890",
                        password = "HappyDays22@",
                        online = true
                    });

                    add(new DM.Client
                    {
                        phone_number = "+79016789012",
                        password = "StarLight3%"
                    });

                    add(new DM.Client
                    {
                        phone_number = "+79015678901",
                        password = "Dreamer!798",
                        online = true
                    });

                    db.SaveChanges();
                }
                // Client

                // --------- Courier ---------
                {
                    Func<DM.Courier, DM.Courier> add = db.Courier.Add;

                    add(new DM.Courier
                    {
                        full_name = "Смирнов Алексей Николаевич",
                        phone_number = "+79001234567",
                        password = "Qwerty1234",
                        online = true,
                    });
                    
                    add(new DM.Courier
                    {
                        full_name = "Коваленко Ольга Владимировна",
                        phone_number = "+79002345678",
                        password = "Asdfgh5678",
                        online = true,
                    });

                    add(new DM.Courier
                    {
                        full_name = "Сидорова Наталья Павловна",
                        phone_number = "+79003456789",
                        password = "Zxcvbn9876",
                        online = true
                    });

                    add(new DM.Courier
                    {
                        full_name = "Морозов Артем Сергеевич",
                        phone_number = "+79004567890",
                        password = "PassWOrd123",
                        online = true
                    });

                    add(new DM.Courier
                    {
                        full_name = "Романова Виктория Леонидовна",
                        phone_number = "+79005678901",
                        password = "12345Abcdef"
                    });
                    
                    db.SaveChanges();
                }
                // Courier

                // --------- Cook ---------
                {
                    Func<DM.Cook, DM.Cook> add = db.Cook.Add;

                    add(new DM.Cook
                    {
                        full_name = "Громова Светлана Валерьевна",
                        phone_number = "+79021234567",
                        password = "CookMaster88!",
                        online = true,
                    });
                    
                    add(new DM.Cook
                    {
                        full_name = "Воробьев Максим Анатольевич",
                        phone_number = "+79022345678",
                        password = "SpiceLover77@",
                        online = true,
                    });

                    add(new DM.Cook
                    {
                        full_name = "Самсонова Ирина Викторовна",
                        phone_number = "+79023456789",
                        password = "YummyDishes99#",
                        online = true,
                    });

                    add(new DM.Cook
                    {
                        full_name = "Никитин Артем Сергеевич",
                        phone_number = "+79024567890",
                        password = "ChefLife2023%"
                    });
                    
                    db.SaveChanges();
                }
                // Cook

                // --------- Order ---------
                {
                    Func<DM.Order, DM.Order> add = db.Order.Add;
                    
                    add(new DM.Order
                    {
                        address = "г. Иваново, ул. Рабфаковская, 34",
                        recipient_name = "Иванов Иван Иванович",
                        creation_date = new DateTime(2024, 03, 15, 11, 30, 00),
                        status = 2,
                        client = db.Client.Single(e => e.id == 1),
                        cook = db.Cook.Single(e => e.id == 1),
                        courier = db.Courier.Single(e => e.id == 2)
                    });

                    add(new DM.Order
                    {
                        address = "г. Иваново, ул. Парижской Коммуны, 13",
                        recipient_name = "Иван",
                        creation_date = new DateTime(2024, 03, 15, 12, 38, 00),
                        status = -1,
                        client = db.Client.Single(e => e.id == 1),
                        cook = db.Cook.Single(e => e.id == 1),
                        courier = db.Courier.Single(e => e.id == 1)
                    });

                    add(new DM.Order
                    {
                        address = "г. Иваново, ул. Красных Зорь, 14",
                        recipient_name = "Шапошникова Екатерина Сергеевна",
                        creation_date = new DateTime(2024, 03, 15, 11, 42, 00),
                        status = 2,
                        client = db.Client.Single(e => e.id == 2),
                        cook = db.Cook.Single(e => e.id == 2),
                        courier = db.Courier.Single(e => e.id == 1)
                    });

                    add(new DM.Order
                    {
                        address = "г. Иваново, ул. 10 Августа, 77",
                        recipient_name = "Снегирев Валентин Игоревич",
                        creation_date = new DateTime(2024, 03, 15, 12, 01, 00),
                        status = 2,
                        client = db.Client.Single(e => e.id == 3),
                        cook = db.Cook.Single(e => e.id == 2),
                        courier = db.Courier.Single(e => e.id == 3)
                    });

                    add(new DM.Order
                    {
                        address = "г. Иваново, ул. Громобоя, 4",
                        recipient_name = "Сидоров Тимур Ильич",
                        creation_date = new DateTime(2024, 03, 17, 19, 25, 00),
                        status = 2,
                        client = db.Client.Single(e => e.id == 4),
                        cook = db.Cook.Single(e => e.id == 3),
                        courier = db.Courier.Single(e => e.id == 2)
                    });
                    
                    db.SaveChanges();
                }
                // Order

                // --------- Pizza_Order ---------
                {
                    Action<DM.Pizza_Order> calcTotal = (po_) =>
                    {
                        po_.cost = po_.pizza.cost * po_.quantity * (decimal)po_.size.cost_mult;
                    };

                    Func<DM.Pizza_Order, DM.Pizza_Order> add = db.Pizza_Order.Add;

                    var po = new DM.Pizza_Order
                    {
                        dough = db.Dough.Single(e => e.name == "Традиционное"),
                        size = db.Pizza_Size.Single(e => e.name == "Маленькая"),
                        order = db.Order.Single(e => e.id == 1),
                        pizza = db.Pizza.Single(e => e.name == "Песто"),
                        quantity = 2
                    };
                    calcTotal(po);
                    add(po);

                    po = new DM.Pizza_Order
                    {
                        dough = db.Dough.Single(e => e.name == "Тонкое"),
                        size = db.Pizza_Size.Single(e => e.name == "Средняя"),
                        order = db.Order.Single(e => e.id == 1),
                        pizza = db.Pizza.Single(e => e.name == "Ветчина и сыр"),
                        quantity = 1
                    };
                    calcTotal(po);
                    add(po);

                    po = new DM.Pizza_Order
                    {
                        dough = db.Dough.Single(e => e.name == "Традиционное"),
                        size = db.Pizza_Size.Single(e => e.name == "Большая"),
                        order = db.Order.Single(e => e.id == 2),
                        pizza = db.Pizza.Single(e => e.name == "Гавайская"),
                        quantity = 1
                    };
                    calcTotal(po);
                    add(po);

                    po = new DM.Pizza_Order
                    {
                        dough = db.Dough.Single(e => e.name == "Традиционное"),
                        size = db.Pizza_Size.Single(e => e.name == "Большая"),
                        order = db.Order.Single(e => e.id == 2),
                        pizza = db.Pizza.Single(e => e.name == "Двойной цыплёнок"),
                        quantity = 1
                    };
                    calcTotal(po);
                    add(po);

                    po = new DM.Pizza_Order
                    {
                        dough = db.Dough.Single(e => e.name == "Традиционное"),
                        size = db.Pizza_Size.Single(e => e.name == "Маленькая"),
                        order = db.Order.Single(e => e.id == 3),
                        pizza = db.Pizza.Single(e => e.name == "Гавайская"),
                        quantity = 3
                    };
                    calcTotal(po);
                    add(po);

                    po = new DM.Pizza_Order
                    {
                        dough = db.Dough.Single(e => e.name == "Тонкое"),
                        size = db.Pizza_Size.Single(e => e.name == "Средняя"),
                        order = db.Order.Single(e => e.id == 4),
                        pizza = db.Pizza.Single(e => e.name == "Ветчина и сыр"),
                        quantity = 2
                    };
                    calcTotal(po);
                    add(po);

                    po = new DM.Pizza_Order
                    {
                        dough = db.Dough.Single(e => e.name == "Традиционное"),
                        size = db.Pizza_Size.Single(e => e.name == "Большая"),
                        order = db.Order.Single(e => e.id == 4),
                        pizza = db.Pizza.Single(e => e.name == "Песто"),
                        quantity = 1
                    };
                    calcTotal(po);
                    add(po);

                    po = new DM.Pizza_Order
                    {
                        dough = db.Dough.Single(e => e.name == "Традиционное"),
                        size = db.Pizza_Size.Single(e => e.name == "Маленькая"),
                        order = db.Order.Single(e => e.id == 5),
                        pizza = db.Pizza.Single(e => e.name == "Двойной цыплёнок"),
                        quantity = 2
                    };
                    calcTotal(po);
                    add(po);

                    po = new DM.Pizza_Order
                    {
                        dough = db.Dough.Single(e => e.name == "Тонкое"),
                        size = db.Pizza_Size.Single(e => e.name == "Средняя"),
                        order = db.Order.Single(e => e.id == 5),
                        pizza = db.Pizza.Single(e => e.name == "Ветчина и сыр"),
                        quantity = 1
                    };
                    calcTotal(po);
                    add(po);

                    po = new DM.Pizza_Order
                    {
                        dough = db.Dough.Single(e => e.name == "Традиционное"),
                        size = db.Pizza_Size.Single(e => e.name == "Средняя"),
                        order = db.Order.Single(e => e.id == 5),
                        pizza = db.Pizza.Single(e => e.name == "Песто"),
                        quantity = 1
                    };
                    calcTotal(po);
                    add(po);
                    
                    db.SaveChanges();
                }
                // Pizza_Order
            }
        }
    }
}
