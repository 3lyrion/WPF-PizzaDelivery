using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Interfaces.Repository;
using DTO = Interfaces.DTO;
using SV = Interfaces.Service;

namespace BLL.Service
{
    public class PizzaSize : SV.IPizzaSize
    {
        IDbRepos db;

        public PizzaSize(IDbRepos database)
        {
            db = database;
        }

        public List<DTO.PizzaSize> GetList()
        {
            return db.Pizza_Size.GetList().Select(i => new DTO.PizzaSize(i)).ToList();
        }
    }
}
