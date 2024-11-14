namespace DAL.Repository
{
    using System;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Linq;
    using DM = DomainModel;
    using R = Interfaces.Repository;

    public class Order : R.IRepository<DM.Order>
    {
        PizzaDeliveryDB db;

        public Order(PizzaDeliveryDB theDB)
        {
            db = theDB;
        }

        public List<DM.Order> getList()
        {
            return db.order.ToList();
        }

        public DM.Order getItem(int id)
        {
            return db.order.Find(id);
        }

        public int create(DM.Order entity)
        {
            return db.order.Add(entity).id;
        }

        public void update(DM.Order entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void delete(int id)
        {
            var entity = db.order.Find(id);
            if (entity != null) db.order.Remove(entity);
        }
    }
}
