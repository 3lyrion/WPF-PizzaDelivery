using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Order_Status orderStatusRep;
        Pizza pizzaRep;
        Pizza_Order pizzaOrderRep;
        Pizza_Size pizzaSizeRep;
        Recipe recipeRep;
        Report reportRep;

        public DbRepos()
        {
            db = new PizzaDeliveryDB();

            clientRep = new Client(db);
            cookRep = new Cook(db);
            courierRep = new Courier(db);
            doughRep = new Dough(db);
            ingredientRep = new Ingredient(db);
            orderRep = new Order(db);
            orderStatusRep = new Order_Status(db);
            pizzaRep = new Pizza(db);
            pizzaOrderRep = new Pizza_Order(db);
            pizzaSizeRep = new Pizza_Size(db);
            recipeRep = new Recipe(db);
            reportRep = new Report(db);
        }

        public R.IRepository<DM.Client> client
        {
            get
            {
                return clientRep;
            }
        }

        public R.IRepository<DM.Cook> cook
        {
            get
            {
                return cookRep;
            }
        }

        public R.IRepository<DM.Courier> courier
        {
            get
            {
                return courierRep;
            }
        }

        public R.IRepository<DM.Dough> dough
        {
            get
            {
                return doughRep;
            }
        }

        public R.IRepository<DM.Ingredient> ingredient
        {
            get
            {
                return ingredientRep;
            }
        }

        public R.IRepository<DM.Order> order
        {
            get
            {
                return orderRep;
            }
        }

        public R.IRepository<DM.Order_Status> order_status
        {
            get
            {
                return orderStatusRep;
            }
        }

        public R.IRepository<DM.Pizza> pizza
        {
            get
            {
                return pizzaRep;
            }
        }

        public R.IRepository<DM.Pizza_Order> pizza_order
        {
            get
            {
                return pizzaOrderRep;
            }
        }

        public R.IRepository<DM.Pizza_Size> pizza_size
        {
            get
            {
                return pizzaSizeRep;
            }
        }

        public R.IRepository<DM.Recipe> recipe
        {
            get
            {
                return recipeRep;
            }
        }

        public R.IReportRepository report
        {
            get
            {
                return reportRep;
            }
        }

        public int save()
        {
            return db.SaveChanges();
        }
    }
}
