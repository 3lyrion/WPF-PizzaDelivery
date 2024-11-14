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

    public class Client : R.IRepository<DM.Client>
    {
        PizzaDeliveryDB db;

        public Client(PizzaDeliveryDB theDB)
        {
            db = theDB;
        }

        public List<DM.Client> getList()
        {
            return db.client.ToList();
        }

        public DM.Client getItem(int id)
        {
            return db.client.Find(id);
        }

        public int create(DM.Client entity)
        {
            return db.client.Add(entity).id;
        }

        public void update(DM.Client entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void delete(int id)
        {
            DM.Client entity = db.client.Find(id);
            if (entity != null) db.client.Remove(entity);
        }
    }
}
