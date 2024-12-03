using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Interfaces.Repository;
using DTO = Interfaces.DTO;
using SV = Interfaces.Service;
using DM = DomainModel;

namespace BLL.Service
{
    public class Pizza : SV.IPizza
    {
        IDbRepos db;

        public Pizza(IDbRepos database)
        {
            db = database;
        }

        public int Create(DTO.Pizza pizzaDto)
        {
            var pizza = new DM.Pizza
            {
                name = pizzaDto.Name,
                cost = pizzaDto.Cost,
                custom = pizzaDto.Custom,
                sales_hit = pizzaDto.SalesHit
            };

            if (pizzaDto.RecipesIds != null)
                pizza.recipe = db.Recipe.GetList().Where(e => pizzaDto.RecipesIds.Contains(e.id)).ToList();

            if (pizzaDto.PizzaOrdersIds != null)
                pizza.pizza_order = db.Pizza_Order.GetList().Where(e => pizzaDto.PizzaOrdersIds.Contains(e.id)).ToList();

            db.Pizza.Create(pizza);

            if (Save())
                return pizza.id;

            return 0;
        }

        public bool Update(DTO.Pizza pizzaDto)
        {
            var pizza = db.Pizza.GetItem(pizzaDto.Id);
            pizza.cost = pizzaDto.Cost;
            pizza.custom = pizzaDto.Custom;
            pizza.name = pizzaDto.Name;
            pizza.sales_hit = pizzaDto.SalesHit;
            pizza.weight = pizzaDto.Weight;

            db.Pizza.Update(pizza);

            return Save();
        }

        public bool Delete(int id)
        {
            var pizza = db.Pizza.GetItem(id);

            if (pizza != null)
            {
                db.Pizza.Delete(id);
                return Save();
            }

            return false;
        }

        public bool Save()
        {
            return db.Save() > 0;
        }

        public List<DTO.Pizza> GetList()
        {
            return db.Pizza.GetList().Select(i => new DTO.Pizza(i)).ToList();
        }

        public List<DTO.Recipe> GetRecipes()
        {
            var recipes = db.Recipe.GetList();

            return recipes.Select(i => new DTO.Recipe(i)).ToList();
        }
    }
}
