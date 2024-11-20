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
            Id = courier.id;
            FullName = courier.full_name;
            Busy = courier.busy;
            Online = courier.online;
            Password = courier.password;
            PhoneNumber = courier.phone_number;
        }
    }
}
