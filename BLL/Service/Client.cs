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

        public bool createClient(DTO.Client clientDto)
        {
            db.client.create(new DM.Client
            {
                full_name = clientDto.full_name,
                online = clientDto.online,
                order = db.order.getList().Where(e => clientDto.ordersIDs.Contains(e.id)).ToList(),
                password = clientDto.password,
                phone_number = clientDto.phone_number
            });

            return save();
        }

        public List<DTO.Client> getAllClients()
        {
            return db.client.getList().Select(i => new DTO.Client(i)).ToList();
        }

        public bool deleteClient(int id)
        {
            var cl = db.client.getItem(id);
            if (cl != null) db.client.delete(id);

            return save();
        }

        public bool save()
        {
            return db.save() > 0;
        }
    }
}
