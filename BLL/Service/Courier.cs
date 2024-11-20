using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Interfaces.Repository;
using DTO = Interfaces.DTO;
using SV = Interfaces.Service;

namespace BLL.Service
{
    public class Courier : SV.ICourier
    {
        IDbRepos db;

        public Courier(IDbRepos database)
        {
            db = database;
        }

        public bool Save()
        {
            return db.Save() > 0;
        }

        public List<DTO.Courier> GetList()
        {
            return db.Courier.GetList().Select(i => new DTO.Courier(i)).ToList();
        }
    }
}
