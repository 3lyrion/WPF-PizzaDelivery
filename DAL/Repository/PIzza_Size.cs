namespace DAL.Repository
{
    using System;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Linq;
    using DM = DomainModel;
    using R = Interfaces.Repository;

    public class Pizza_Size : R.IRepository<DM.Pizza_Size>
    {
        PizzaDeliveryDB db;

        public Pizza_Size(PizzaDeliveryDB theDB)
        {
            db = theDB;
        }

        public List<DM.Pizza_Size> getList()
        {
            return db.pizza_size.ToList();
        }

        public DM.Pizza_Size getItem(int id)
        {
            return db.pizza_size.Find(id);
        }

        public int create(DM.Pizza_Size entity)
        {
            return db.pizza_size.Add(entity).id;
        }

        public void update(DM.Pizza_Size entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void delete(int id)
        {
            var entity = db.pizza_size.Find(id);
            if (entity != null) db.pizza_size.Remove(entity);
        }
    }
}
