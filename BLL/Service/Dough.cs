using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Interfaces.Repository;
using DTO = Interfaces.DTO;
using SV = Interfaces.Service;

namespace BLL.Service
{
    public class Dough : SV.IDough
    {
        IDbRepos db;

        public Dough(IDbRepos database)
        {
            db = database;
        }

        public List<DTO.Dough> GetList()
        {
            return db.Dough.GetList().Select(i => new DTO.Dough(i)).ToList();
        }
    }
}
