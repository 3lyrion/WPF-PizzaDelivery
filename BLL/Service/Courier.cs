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
    public class Courier : SV.ICourier
    {
        IDbRepos db;

        public Courier(IDbRepos database)
        {
            db = database;
        }

        public bool save()
        {
            return db.save() > 0;
        }

        public List<DTO.Courier> getAllCouriers()
        {
            return db.courier.getList().Select(i => new DTO.Courier(i)).ToList();
        }
    }
}
