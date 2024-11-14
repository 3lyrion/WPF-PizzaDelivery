using System;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;

namespace Interfaces.DTO
{
    public partial class Client : Base.User
    {
        public Client() { }

        public Client(DM.Client client)
        {
            id = client.id;
            full_name = client.full_name;
            online = client.online;
            password = client.password;
            phone_number = client.phone_number;
            ordersIDs = client.order.Select(e => e.id).ToList();
        }

        public List<int> ordersIDs;
    }
}
