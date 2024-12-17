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
            var list = db.Courier.ToList();
            list.ForEach((e) => db.Entry(e).Reload());

            return list;
        }

        public DM.Courier GetItem(int id)
        {
            var e = db.Courier.Find(id);
            db.Entry(e).Reload();

            return e;
        }

        public void Create(DM.Courier entity)
        {
            db.Courier.Add(entity);
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
