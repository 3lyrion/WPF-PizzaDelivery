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

    public class Courier : R.IRepository<DM.Courier>
    {
        PizzaDeliveryDB db;

        public Courier(PizzaDeliveryDB theDB)
        {
            db = theDB;
        }

        public List<DM.Courier> getList()
        {
            return db.courier.ToList();
        }

        public DM.Courier getItem(int id)
        {
            return db.courier.Find(id);
        }

        public int create(DM.Courier entity)
        {
            return db.courier.Add(entity).id;
        }

        public void update(DM.Courier entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void delete(int id)
        {
            DM.Courier entity = db.courier.Find(id);
            if (entity != null) db.courier.Remove(entity);
        }
    }
}
