using System;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;

namespace Interfaces.DTO
{
    public class Recipe
    {
        public Recipe() { }

        public Recipe(DM.Recipe recipe)
        {
            Id = recipe.id;
            IngredientId = recipe.ingredient.id;
            PizzaId = recipe.pizza.id;
            Quantity = recipe.quantity;
        }

        public int Id { get; set; }

        public int IngredientId { get; set; }

        public int PizzaId { get; set; }

        public int Quantity { get; set; }
    }
}
