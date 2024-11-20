using System;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;

namespace Interfaces.DTO
{
    public class Client : Base.User
    {
        public Client() { }

        public Client(DM.Client client)
        {
            Id = client.id;
            FullName = client.full_name;
            Online = client.online;
            Password = client.password;
            PhoneNumber = client.phone_number;
            OrdersIDs = client.order.Select(e => e.id).ToList();
        }

        public List<int> OrdersIDs { get; set; }
    }
}
