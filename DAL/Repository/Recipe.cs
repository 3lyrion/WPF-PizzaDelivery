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

    public class Recipe : R.IRepository<DM.Recipe>
    {
        PizzaDeliveryDB db;

        public Recipe(PizzaDeliveryDB theDB)
        {
            db = theDB;
        }

        public List<DM.Recipe> getList()
        {
            return db.recipe.ToList();
        }

        public DM.Recipe getItem(int id)
        {
            return db.recipe.Find(id);
        }

        public int create(DM.Recipe entity)
        {
            return db.recipe.Add(entity).id;
        }

        public void update(DM.Recipe entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void delete(int id)
        {
            var entity = db.recipe.Find(id);
            if (entity != null) db.recipe.Remove(entity);
        }
    }
}
