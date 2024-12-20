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
    }
}
