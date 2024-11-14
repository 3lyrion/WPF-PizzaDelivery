namespace DAL.Repository
{
    using System;
    using System.Data;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Linq;
    using DM = DomainModel;
    using R = Interfaces.Repository;

    public class Order_Status : R.IRepository<DM.Order_Status>
    {
        PizzaDeliveryDB db;

        public Order_Status(PizzaDeliveryDB theDB)
        {
            db = theDB;
        }

        public List<DM.Order_Status> getList()
        {
            return db.order_status.ToList();
        }

        public DM.Order_Status getItem(int id)
        {
            return db.order_status.Find(id);
        }

        public int create(DM.Order_Status entity)
        {
            return db.order_status.Add(entity).id;
        }

        public void update(DM.Order_Status entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void delete(int id)
        {
            var entity = db.order_status.Find(id);
            if (entity != null) db.order_status.Remove(entity);
        }
    }
}
