using System;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;

namespace Interfaces.DTO
{
    public enum OrderStatus
    {
        Cancellation = -1,
        Stucked = 0,
        Preparation,
        Delivery,
        Success,
    }

    public class Order
    {
        public Order() { }

        public Order(DM.Order order)
        {
            Id = order.id;
            CreationDate = order.creation_date;
            Cost = order.cost;
            Address = order.address;
            RecipientName = order.recipient_name;
            ClientId = order.client.id;
            Status = (OrderStatus)order.status;
            if (order.cook != null) CookId = order.cook.id;
            if (order.courier != null) CourierId = order.courier.id;
            PizzaOrdersIds = order.pizza_order.Select(e => e.id).ToList();
        }

        public int Id { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public decimal Cost { get; set; }

        public string Address { get; set; }

        public string RecipientName { get; set; }

        public OrderStatus Status { get; set; }

        public int ClientId { get; set; }

        public int CookId { get; set; }

        public int CourierId { get; set; }

        public List<int> PizzaOrdersIds { get; set; }
    }
}
