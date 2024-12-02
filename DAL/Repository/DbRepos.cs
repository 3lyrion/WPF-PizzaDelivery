using System;
using DM = DomainModel;
using R = Interfaces.Repository;

namespace DAL.Repository
{
    public class DbRepos : R.IDbRepos
    {
        PizzaDeliveryDB db;

        Client clientRep;
        Cook cookRep;
        Courier courierRep;
        Dough doughRep;
        Ingredient ingredientRep;
        Order orderRep;
        Pizza pizzaRep;
        Pizza_Order pizzaOrderRep;
        Pizza_Size pizzaSizeRep;
        Recipe recipeRep;
        Report reportRep;
        Transaction transactionRep;

        public DbRepos()
        {
            db = new PizzaDeliveryDB();

            clientRep = new Client(db);
            cookRep = new Cook(db);
            courierRep = new Courier(db);
            doughRep = new Dough(db);
            ingredientRep = new Ingredient(db);
            orderRep = new Order(db);
            pizzaRep = new Pizza(db);
            pizzaOrderRep = new Pizza_Order(db);
            pizzaSizeRep = new Pizza_Size(db);
            recipeRep = new Recipe(db);
            reportRep = new Report(db);
            transactionRep = new Transaction(db);
        }

        public R.IRepository<DM.Client> Client
        {
            get
            {
                return clientRep;
            }
        }

        public R.IRepository<DM.Cook> Cook
        {
            get
            {
                return cookRep;
            }
        }

        public R.IRepository<DM.Courier> Courier
        {
            get
            {
                return courierRep;
            }
        }

        public R.IRepository<DM.Dough> Dough
        {
            get
            {
                return doughRep;
            }
        }

        public R.IRepository<DM.Ingredient> Ingredient
        {
            get
            {
                return ingredientRep;
            }
        }

        public R.IRepository<DM.Order> Order
        {
            get
            {
                return orderRep;
            }
        }

        public R.IRepository<DM.Pizza> Pizza
        {
            get
            {
                return pizzaRep;
            }
        }

        public R.IRepository<DM.Pizza_Order> Pizza_Order
        {
            get
            {
                return pizzaOrderRep;
            }
        }

        public R.IRepository<DM.Pizza_Size> Pizza_Size
        {
            get
            {
                return pizzaSizeRep;
            }
        }

        public R.IRepository<DM.Recipe> Recipe
        {
            get
            {
                return recipeRep;
            }
        }

        public R.IReportRepository Report
        {
            get
            {
                return reportRep;
            }
        }

        public R.ITransactionRepository Transaction
        {
            get
            {
                return transactionRep;
            }
        }

        public int Save()
        {
            return db.SaveChanges();
        }
    }
}
