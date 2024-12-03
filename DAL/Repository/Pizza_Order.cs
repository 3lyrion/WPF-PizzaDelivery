using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;
using R = Interfaces.Repository;

namespace DAL.Repository
{
    public class Pizza_Order : R.IRepository<DM.Pizza_Order>
    {
        PizzaDeliveryDB db;

        public Pizza_Order(PizzaDeliveryDB theDB)
        {
            db = theDB;
        }

        public List<DM.Pizza_Order> GetList()
        {
            return db.Pizza_Order.ToList();
        }

        public DM.Pizza_Order GetItem(int id)
        {
            return db.Pizza_Order.Find(id);
        }

        public void Create(DM.Pizza_Order entity)
        {
            db.Pizza_Order.Add(entity);
        }

        public void Update(DM.Pizza_Order entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var entity = db.Pizza_Order.Find(id);
            if (entity != null) db.Pizza_Order.Remove(entity);
        }
    }
}
