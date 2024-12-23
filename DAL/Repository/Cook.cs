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
            var list = db.Cook.ToList();
            list.ForEach((e) => db.Entry(e).Reload());

            return list;
        }

        public DM.Cook GetItem(int id)
        {
            var e = db.Cook.Find(id);
            db.Entry(e).Reload();

            return e;
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
