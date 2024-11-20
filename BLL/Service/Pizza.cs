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
            var recipes = db.Recipe.GetList().Where(e => pizzaDto.RecipesIds.Contains(e.id)).ToList();

            var pizza = new DM.Pizza
            {
                name = pizzaDto.Name,
                cost = pizzaDto.Cost,
                custom = pizzaDto.Custom,
                pizza_order = db.Pizza_Order.GetList().Where(e => pizzaDto.PizzaOrdersIds.Contains(e.id)).ToList(),
                recipe = recipes,
                sales_hit = pizzaDto.SalesHit,
                weight = recipes.Sum(e => e.quantity)
            };

            db.Pizza.Create(pizza);

            if (Save())
                return pizza.id;

            return 0;
        }

        public bool Save()
        {
            return db.Save() > 0;
        }

        public List<DTO.Pizza> GetList()
        {
            return db.Pizza.GetList().Select(i => new DTO.Pizza(i)).ToList();
        }
    }
}
