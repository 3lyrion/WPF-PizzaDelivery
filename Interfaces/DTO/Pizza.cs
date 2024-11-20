using System;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;

namespace Interfaces.DTO
{
    public class Pizza
    {
        public Pizza() { }

        public Pizza(DM.Pizza pizza)
        {
            Id = pizza.id;
            Custom = pizza.custom;
            SalesHit = pizza.sales_hit;
            Name = pizza.name;
            Cost = pizza.cost;
            Weight = pizza.weight;
            PizzaOrdersIds = pizza.pizza_order.Select(e => e.id).ToList();
            RecipesIds = pizza.recipe.Select(e => e.id).ToList();
        }

        public int Id { get; set; }

        public bool Custom { get; set; }

        public bool SalesHit { get; set; }

        public string Name { get; set; }

        public decimal Cost { get; set; }

        public int Weight { get; set; }

        public List<int> RecipesIds { get; set; }

        public List<int> PizzaOrdersIds { get; set; }
    }
}
