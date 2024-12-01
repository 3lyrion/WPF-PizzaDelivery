using System;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;

namespace Interfaces.DTO
{
    public class PizzaOrder
    {
        public PizzaOrder() { }

        public PizzaOrder(DM.Pizza_Order pizza_order)
        {
            Id = pizza_order.id;
            Cost = pizza_order.cost;
            Quantity = pizza_order.quantity;
            DoughId = pizza_order.dough.id;
            SizeId = pizza_order.size.id;
            PizzaId = pizza_order.pizza.id;
            if (pizza_order.order != null) OrderId = pizza_order.order.id;
        }

        public int Id { get; set; }

        public decimal Cost { get; set; }
        
        public int Quantity { get; set; }

        public int DoughId { get; set; }

        public int SizeId { get; set; }

        public int PizzaId { get; set; }

        public int? OrderId { get; set; }
    }
}
