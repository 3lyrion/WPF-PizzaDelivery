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

    public class Cook : R.IRepository<DM.Cook>
    {
        PizzaDeliveryDB db;

        public Cook(PizzaDeliveryDB theDB)
        {
            db = theDB;
        }

        public List<DM.Cook> getList()
        {
            return db.cook.ToList();
        }

        public DM.Cook getItem(int id)
        {
            return db.cook.Find(id);
        }

        public int create(DM.Cook entity)
        {
            return db.cook.Add(entity).id;
        }

        public void update(DM.Cook entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void delete(int id)
        {
            var entity = db.cook.Find(id);
            if (entity != null) db.cook.Remove(entity);
        }
    }
}
