using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;
using R = Interfaces.Repository;

namespace DAL.Repository
{
    public class Cook : R.IRepository<DM.Cook>
    {
        PizzaDeliveryDB db;

        public Cook(PizzaDeliveryDB theDB)
        {
            db = theDB;
        }

        public List<DM.Cook> GetList()
        {
            return db.Cook.ToList();
        }

        public DM.Cook GetItem(int id)
        {
            return db.Cook.Find(id);
        }

        public void Create(DM.Cook entity)
        {
            db.Cook.Add(entity);
        }

        public void Update(DM.Cook entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var entity = db.Cook.Find(id);
            if (entity != null) db.Cook.Remove(entity);
        }
    }
}
