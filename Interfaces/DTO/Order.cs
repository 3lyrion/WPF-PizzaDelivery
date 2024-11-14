using System;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;

namespace Interfaces.DTO
{
    public class Order
    {
        public Order() { }

        public Order(DM.Order order)
        {
            id = order.id;
            creation_date = order.creation_date;
            cost = order.cost;
            address = order.address;
            //status_id = order.status.id;
            client_id = order.client.id;
            if (order.cook != null) cook_id = order.cook.id;
            if (order.courier != null) courier_id = order.courier.id;
            pizza_order_IDs = order.pizza_order.Select(e => e.id).ToList();
        }

        public int id { get; set; }

        public DateTime? creation_date { get; set; }

        public decimal? cost { get; set; }

        public string address { get; set; }

    //    public int status_id { get; set; }

        public int client_id { get; set; }

        public int cook_id { get; set; }

        public int courier_id { get; set; }

        public List<int> pizza_order_IDs { get; set; }
    }
}
