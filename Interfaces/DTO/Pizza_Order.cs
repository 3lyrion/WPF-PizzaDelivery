using System;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;

namespace Interfaces.DTO
{
    public class Pizza_Order
    {
        public Pizza_Order() { }

        public Pizza_Order(DM.Pizza_Order pizza_order)
        {
            id = pizza_order.id;
            if (pizza_order.cost.HasValue) cost = pizza_order.cost.Value;
            dough_id = pizza_order.dough.id;
            pizza_id = pizza_order.pizza.id;
            quantity = pizza_order.quantity;
            size_id = pizza_order.size.id;

            if (pizza_order.order != null) order_id = pizza_order.order.id;
        }

        public int id { get; set; }

        public int quantity { get; set; }

        public decimal cost { get; set; }

        public int dough_id { get; set; }

        public int size_id { get; set; }

        public int pizza_id { get; set; }

        public int order_id { get; set; }
    }
}
