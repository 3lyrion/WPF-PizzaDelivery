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
    public class Pizza_Size : SV.IPizza_Size
    {
        IDbRepos db;

        public Pizza_Size(IDbRepos database)
        {
            db = database;
        }

        public List<DTO.Pizza_Size> getAllSizes()
        {
            return db.pizza_size.getList().Select(i => new DTO.Pizza_Size(i)).ToList();
        }
    }
}
