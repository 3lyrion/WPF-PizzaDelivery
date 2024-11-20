using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;
using R = Interfaces.Repository;

namespace DAL.Repository
{
    public class Recipe : R.IRepository<DM.Recipe>
    {
        PizzaDeliveryDB db;

        public Recipe(PizzaDeliveryDB theDB)
        {
            db = theDB;
        }

        public List<DM.Recipe> GetList()
        {
            return db.Recipe.ToList();
        }

        public DM.Recipe GetItem(int id)
        {
            return db.Recipe.Find(id);
        }

        public int Create(DM.Recipe entity)
        {
            return db.Recipe.Add(entity).id;
        }

        public void Update(DM.Recipe entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var entity = db.Recipe.Find(id);
            if (entity != null) db.Recipe.Remove(entity);
        }
    }
}
