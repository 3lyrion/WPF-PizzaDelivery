using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;
using R = Interfaces.Repository;

namespace DAL.Repository
{
    public class Client : R.IRepository<DM.Client>
    {
        PizzaDeliveryDB db;

        public Client(PizzaDeliveryDB theDB)
        {
            db = theDB;
        }

        public List<DM.Client> GetList()
        {
            var list = db.Client.ToList();
            list.ForEach((e) => db.Entry(e).Reload());

            return list;
        }

        public DM.Client GetItem(int id)
        {
            var e = db.Client.Find(id);
            db.Entry(e).Reload();

            return e;
        }

        public void Create(DM.Client entity)
        {
            db.Client.Add(entity);
        }

        public void Update(DM.Client entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            DM.Client entity = db.Client.Find(id);
            if (entity != null) db.Client.Remove(entity);
        }
    }
}
