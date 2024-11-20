using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;
using R = Interfaces.Repository;

namespace DAL.Repository
{
    public class Order : R.IRepository<DM.Order>
    {
        PizzaDeliveryDB db;

        public Order(PizzaDeliveryDB theDB)
        {
            db = theDB;
        }

        public List<DM.Order> GetList()
        {
            return db.Order.ToList();
        }

        public DM.Order GetItem(int id)
        {
            return db.Order.Find(id);
        }

        public int Create(DM.Order entity)
        {
            return db.Order.Add(entity).id;
        }

        public void Update(DM.Order entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var entity = db.Order.Find(id);
            if (entity != null) db.Order.Remove(entity);
        }
    }
}
