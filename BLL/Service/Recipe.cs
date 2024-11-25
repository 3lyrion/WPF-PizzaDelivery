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
    public class Recipe : SV.IRecipe
    {
        IDbRepos db;

        public Recipe(IDbRepos database)
        {
            db = database;
        }

        public int Create(DTO.Recipe recipeDto)
        {
            var recipe = new DM.Recipe
            {
                ingredient = db.Ingredient.GetItem(recipeDto.IngredientId),
                pizza = db.Pizza.GetItem(recipeDto.PizzaId),
                quantity = recipeDto.Quantity
            };

            db.Recipe.Create(recipe);

            if (Save())
                return recipe.id;

            return 0;
        }

        public bool Update(DTO.Recipe recipeDto)
        {
            var recipe = db.Recipe.GetItem(recipeDto.Id);
            recipe.ingredient = db.Ingredient.GetItem(recipeDto.IngredientId);
            recipe.pizza = db.Pizza.GetItem(recipeDto.PizzaId);
            recipe.quantity = recipeDto.Quantity;

            db.Recipe.Update(recipe);

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

        public List<DTO.Recipe> GetList()
        {
            return db.Recipe.GetList().Select(i => new DTO.Recipe(i)).ToList();
        }
    }
}
