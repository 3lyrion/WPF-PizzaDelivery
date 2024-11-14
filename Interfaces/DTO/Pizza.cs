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
            this.cost = pizza.cost;
            this.custom = pizza.custom;
            this.id = pizza.id;
            this.name = pizza.name;
            this.pizza_order_IDs = pizza.pizza_order.Select(e => e.id).ToList();
            this.recipesIDs = pizza.recipe.Select(e => e.id).ToList();
            this.sales_hit = pizza.sales_hit;
            this.weight = pizza.weight;
        }

        public int id { get; set; }

        public bool custom { get; set; }

        public bool sales_hit { get; set; }

        public string name { get; set; }

        public decimal cost { get; set; }

        public int? weight { get; set; }

        public List<int> recipesIDs { get; set; }
        public List<int> pizza_order_IDs { get; set; }
    }
}
