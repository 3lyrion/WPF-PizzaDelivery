using System;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;

namespace Interfaces.DTO
{
    public class Ingredient
    {
        public Ingredient() { }

        public Ingredient(DM.Ingredient ingredient)
        {
            Id = ingredient.id;
            Name = ingredient.name;
            Cost = ingredient.cost;
            HiddenFromClient = ingredient.hidden_from_client;
            InStock = ingredient.in_stock;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Cost { get; set; }

        public bool HiddenFromClient { get; set; } = false;

        public bool InStock { get; set; } = false;
    }
}