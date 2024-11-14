namespace DAL.Repository
{
    using System;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Linq;
    using DM = DomainModel;
    using R = Interfaces.Repository;

    public class Dough : R.IRepository<DM.Dough>
    {
        PizzaDeliveryDB db;

        public Dough(PizzaDeliveryDB theDB)
        {
            db = theDB;
        }

        public List<DM.Dough> getList()
        {
            return db.dough.ToList();
        }

        public DM.Dough getItem(int id)
        {
            return db.dough.Find(id);
        }

        public int create(DM.Dough entity)
        {
            return db.dough.Add(entity).id;
        }

        public void update(DM.Dough entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void delete(int id)
        {
            var entity = db.dough.Find(id);
            if (entity != null) db.dough.Remove(entity);
        }
    }
}
