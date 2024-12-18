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
            var list = db.Order.ToList();
            list.ForEach((e) => db.Entry(e).Reload());

            return list;
        }

        public DM.Order GetItem(int id)
        {
            var e = db.Order.Find(id);
            db.Entry(e).Reload();

            return e;
        }

        public void Create(DM.Order entity)
        {
            db.Order.Add(entity);
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
