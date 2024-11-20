using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;
using R = Interfaces.Repository;

namespace DAL.Repository
{
    public class Pizza_Size : R.IRepository<DM.Pizza_Size>
    {
        PizzaDeliveryDB db;

        public Pizza_Size(PizzaDeliveryDB theDB)
        {
            db = theDB;
        }

        public List<DM.Pizza_Size> GetList()
        {
            return db.Pizza_Size.ToList();
        }

        public DM.Pizza_Size GetItem(int id)
        {
            return db.Pizza_Size.Find(id);
        }

        public int Create(DM.Pizza_Size entity)
        {
            return db.Pizza_Size.Add(entity).id;
        }

        public void Update(DM.Pizza_Size entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var entity = db.Pizza_Size.Find(id);
            if (entity != null) db.Pizza_Size.Remove(entity);
        }
    }
}
