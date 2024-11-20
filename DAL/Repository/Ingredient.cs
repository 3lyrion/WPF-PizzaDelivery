using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;
using R = Interfaces.Repository;

namespace DAL.Repository
{
    public class Ingredient : R.IRepository<DM.Ingredient>
    {
        PizzaDeliveryDB db;

        public Ingredient(PizzaDeliveryDB theDB)
        {
            db = theDB;
        }

        public List<DM.Ingredient> GetList()
        {
            return db.Ingredient.ToList();
        }

        public DM.Ingredient GetItem(int id)
        {
            return db.Ingredient.Find(id);
        }

        public int Create(DM.Ingredient entity)
        {
            return db.Ingredient.Add(entity).id;
        }

        public void Update(DM.Ingredient entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var entity = db.Ingredient.Find(id);
            if (entity != null) db.Ingredient.Remove(entity);
        }
    }
}
