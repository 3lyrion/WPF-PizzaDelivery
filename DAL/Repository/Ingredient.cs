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

    public class Ingredient : R.IRepository<DM.Ingredient>
    {
        PizzaDeliveryDB db;

        public Ingredient(PizzaDeliveryDB theDB)
        {
            db = theDB;
        }

        public List<DM.Ingredient> getList()
        {
            return db.ingredient.ToList();
        }

        public DM.Ingredient getItem(int id)
        {
            return db.ingredient.Find(id);
        }

        public int create(DM.Ingredient entity)
        {
            return db.ingredient.Add(entity).id;
        }

        public void update(DM.Ingredient entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void delete(int id)
        {
            var entity = db.ingredient.Find(id);
            if (entity != null) db.ingredient.Remove(entity);
        }
    }
}
