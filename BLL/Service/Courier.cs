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

        public int Create(DTO.Courier courierDto)
        {
            var courier = new DM.Courier
            {
                full_name = courierDto.FullName,
                busy = courierDto.Busy,
                online = courierDto.Online,
                password = courierDto.Password,
                phone_number = courierDto.PhoneNumber
            };

            courier.id = db.Courier.Create(courier);

            if (Save())
                return courier.id;

            return 0;
        }

        public bool Update(DTO.Courier courierDto)
        {
            var courier = db.Courier.GetItem(courierDto.Id);
            courier.full_name = courierDto.FullName;
            courier.busy = courierDto.Busy;
            courier.online = courierDto.Online;
            courier.password = courierDto.Password;
            courier.phone_number = courierDto.PhoneNumber;

            db.Courier.Update(courier);

            return Save();
        }

        public bool Delete(int id)
        {
            var courier = db.Courier.GetItem(id);

            if (courier != null)
            {
                db.Courier.Delete(id);
                return Save();
            }

            return false;
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
