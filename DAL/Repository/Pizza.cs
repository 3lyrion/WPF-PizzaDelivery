namespace DAL.Repository
{
    using System;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Linq;
    using DM = DomainModel;
    using R = Interfaces.Repository;

    public class Pizza : R.IRepository<DM.Pizza>
    {
        PizzaDeliveryDB db;

        public Pizza(PizzaDeliveryDB theDB)
        {
            db = theDB;
        }

        public List<DM.Pizza> getList()
        {
            return db.pizza.ToList();
        }

        public DM.Pizza getItem(int id)
        {
            return db.pizza.Find(id);
        }

        public int create(DM.Pizza entity)
        {
            return db.pizza.Add(entity).id;
        }

        public void update(DM.Pizza entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void delete(int id)
        {
            var entity = db.pizza.Find(id);
            if (entity != null) db.pizza.Remove(entity);
        }
    }
}
