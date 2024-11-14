using System;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;

namespace Interfaces.DTO
{
    public partial class Courier : Base.Employee
    {
        public Courier() { }

        public Courier(DM.Courier courier)
        {
            id = courier.id;
            full_name = courier.full_name;
            online = courier.online;
            busy = courier.busy;
            password = courier.password;
            phone_number = courier.phone_number;
        }
    }
}
