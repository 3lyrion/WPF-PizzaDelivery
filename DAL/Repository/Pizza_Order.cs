namespace DAL.Repository
{
    using System;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Linq;
    using DM = DomainModel;
    using R = Interfaces.Repository;

    public class Pizza_Order : R.IRepository<DM.Pizza_Order>
    {
        PizzaDeliveryDB db;

        public Pizza_Order(PizzaDeliveryDB theDB)
        {
            db = theDB;
        }

        public List<DM.Pizza_Order> getList()
        {
            return db.pizza_order.ToList();
        }

        public DM.Pizza_Order getItem(int id)
        {
            return db.pizza_order.Find(id);
        }

        public int create(DM.Pizza_Order entity)
        {
            return db.pizza_order.Add(entity).id;
        }

        public void update(DM.Pizza_Order entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void delete(int id)
        {
            var entity = db.pizza_order.Find(id);
            if (entity != null) db.pizza_order.Remove(entity);
        }
    }
}
