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
    public class Ingredient : SV.IIngredient
    {
        IDbRepos db;

        public Ingredient(IDbRepos database)
        {
            db = database;
        }

        public List<DTO.Ingredient> GetList()
        {
            return db.Ingredient.GetList().Select(i => new DTO.Ingredient(i)).ToList();
        }
    }
}
