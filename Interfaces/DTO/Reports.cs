using System;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;

namespace Interfaces.DTO
{
    public class OnlineClientOrders
    {
        public int id { get; set; }

        public string full_name { get; set; }

        public string order_ids { get; set; }
    }

    public class OnlineCourierOrder
    {
        public int id { get; set; }

        public string full_name { get; set; }

        public int order_id { get; set; }
    }
}
