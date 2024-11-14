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

        public bool createPizza(DTO.Pizza pizzaDto)
        {
            var recipes = db.recipe.getList().Where(e => pizzaDto.recipesIDs.Contains(e.id)).ToList();

            db.pizza.create(new DM.Pizza
            {
                name = pizzaDto.name,
                cost = pizzaDto.cost,
                custom = pizzaDto.custom,
                pizza_order = db.pizza_order.getList().Where(e => pizzaDto.pizza_order_IDs.Contains(e.id)).ToList(),
                recipe = recipes,
                sales_hit = pizzaDto.sales_hit,
                weight = recipes.Sum(e => e.quantity)
            });

            if (db.save() > 0)
                return true;

            return false;
        }


        public List<DTO.Pizza> getAllPizzas()
        {
            return db.pizza.getList().Select(i => new DTO.Pizza(i)).ToList();
        }
    }
}
