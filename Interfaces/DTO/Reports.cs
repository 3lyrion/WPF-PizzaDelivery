using System;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;

namespace Interfaces.DTO
{
    public class OnlineClientOrders
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string OrdersIds { get; set; }
    }

    public class OnlineCourierOrder
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public int OrderId { get; set; }
    }
}
