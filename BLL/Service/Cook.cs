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
    public class Cook : SV.ICook
    {
        IDbRepos db;

        public Cook(IDbRepos database)
        {
            db = database;
        }

        public int Create(DTO.Cook cookDto)
        {
            var cook = new DM.Cook
            {
                full_name = cookDto.FullName,
                busy = cookDto.Busy,
                online = cookDto.Online,
                password = cookDto.Password,
                phone_number = cookDto.PhoneNumber
            };

            cook.id = db.Cook.Create(cook);

            if (Save())
                return cook.id;

            return 0;
        }

        public bool Update(DTO.Cook cookDto)
        {
            var cook = db.Cook.GetItem(cookDto.Id);
            cook.full_name = cookDto.FullName;
            cook.busy = cookDto.Busy;
            cook.online = cookDto.Online;
            cook.password = cookDto.Password;
            cook.phone_number = cookDto.PhoneNumber;

            db.Cook.Update(cook);

            return Save();
        }

        public bool Delete(int id)
        {
            var cook = db.Cook.GetItem(id);

            if (cook != null)
            {
                db.Cook.Delete(id);
                return Save();
            }

            return false;
        }

        public bool Save()
        {
            return db.Save() > 0;
        }

        public List<DTO.Cook> GetList()
        {
            return db.Cook.GetList().Select(i => new DTO.Cook(i)).ToList();
        }
    }
}
