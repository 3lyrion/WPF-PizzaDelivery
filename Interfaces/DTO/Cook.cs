using System;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;

namespace Interfaces.DTO
{
    public partial class Cook : Base.Employee
    {
        public Cook() { }

        public Cook(DM.Cook cook)
        {
            Id = cook.id;
            FullName = cook.full_name;
            Busy = cook.busy;
            Online = cook.online;
            Password = cook.password;
            PhoneNumber = cook.phone_number;
        }
    }
}
