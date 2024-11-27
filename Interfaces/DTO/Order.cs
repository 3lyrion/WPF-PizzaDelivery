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
            Id = order.id;
            CreationDate = order.creation_date;
            Cost = order.cost;
            Address = order.address;
            //status_id = order.status.id;
            ClientId = order.client.id;
            if (order.cook != null) CookId = order.cook.id;
            if (order.courier != null) CourierId = order.courier.id;
            PizzaOrdersIds = order.pizza_order.Select(e => e.id).ToList();
        }

        public int Id { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public decimal? Cost { get; set; }

        public string Address { get; set; }

    //    public int status_id { get; set; }

        public int ClientId { get; set; }

        public int? CookId { get; set; }

        public int? CourierId { get; set; }

        public List<int> PizzaOrdersIds { get; set; }
    }
}
