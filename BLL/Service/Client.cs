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
    public class Client : SV.IClient
    {
        IDbRepos db;

        public Client(IDbRepos database)
        {
            db = database;
        }

        public int Create(DTO.Client clientDto)
        {
            var client = new DM.Client
            {
                online = clientDto.Online,
                order = db.Order.GetList().Where(e => clientDto.OrdersIDs.Contains(e.id)).ToList(),
                password = clientDto.Password,
                phone_number = clientDto.PhoneNumber
            };

            db.Client.Create(client);

            if (Save())
                return client.id;

            return 0;
        }

        public bool Update(DTO.Client clientDto)
        {
            var client = db.Client.GetItem(clientDto.Id);
            client.online = clientDto.Online;
            client.password = clientDto.Password;
            client.phone_number = clientDto.PhoneNumber;

            db.Client.Update(client);

            return Save();
        }

        public bool Delete(int id)
        {
            var cl = db.Client.GetItem(id);
            if (cl != null) db.Client.Delete(id);

            return Save();
        }

        public bool Save()
        {
            return db.Save() > 0;
        }

        public List<DTO.Client> GetList()
        {
            return db.Client.GetList().Select(i => new DTO.Client(i)).ToList();
        }
    }
}
