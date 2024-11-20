using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;
using R = Interfaces.Repository;

namespace DAL.Repository
{
    public class Courier : R.IRepository<DM.Courier>
    {
        PizzaDeliveryDB db;

        public Courier(PizzaDeliveryDB theDB)
        {
            db = theDB;
        }

        public List<DM.Courier> GetList()
        {
            return db.Courier.ToList();
        }

        public DM.Courier GetItem(int id)
        {
            return db.Courier.Find(id);
        }

        public int Create(DM.Courier entity)
        {
            return db.Courier.Add(entity).id;
        }

        public void Update(DM.Courier entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            DM.Courier entity = db.Courier.Find(id);
            if (entity != null) db.Courier.Remove(entity);
        }
    }
}
