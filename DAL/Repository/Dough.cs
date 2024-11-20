using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;
using R = Interfaces.Repository;

namespace DAL.Repository
{
    public class Dough : R.IRepository<DM.Dough>
    {
        PizzaDeliveryDB db;

        public Dough(PizzaDeliveryDB theDB)
        {
            db = theDB;
        }

        public List<DM.Dough> GetList()
        {
            return db.Dough.ToList();
        }

        public DM.Dough GetItem(int id)
        {
            return db.Dough.Find(id);
        }

        public int Create(DM.Dough entity)
        {
            return db.Dough.Add(entity).id;
        }

        public void Update(DM.Dough entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var entity = db.Dough.Find(id);
            if (entity != null) db.Dough.Remove(entity);
        }
    }
}
