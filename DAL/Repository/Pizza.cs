using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;
using R = Interfaces.Repository;

namespace DAL.Repository
{
    public class Pizza : R.IRepository<DM.Pizza>
    {
        PizzaDeliveryDB db;

        public Pizza(PizzaDeliveryDB theDB)
        {
            db = theDB;
        }

        public List<DM.Pizza> GetList()
        {
            return db.Pizza.ToList();
        }

        public DM.Pizza GetItem(int id)
        {
            return db.Pizza.Find(id);
        }

        public int Create(DM.Pizza entity)
        {
            return db.Pizza.Add(entity).id;
        }

        public void Update(DM.Pizza entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var entity = db.Pizza.Find(id);
            if (entity != null) db.Pizza.Remove(entity);
        }
    }
}
