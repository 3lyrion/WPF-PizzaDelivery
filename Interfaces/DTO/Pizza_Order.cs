using System;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;

namespace Interfaces.DTO
{
    public class Pizza_Order
    {
        public delegate void Creation(Pizza_Order poDto);
        public static event Creation? creation;

        public delegate void CostChangeHandler();
        public event CostChangeHandler? costChange;

        public delegate void QuantityChangeHandler();
        public event QuantityChangeHandler? quantityChange;

        decimal m_cost;
        int m_quantity;

        public Pizza_Order() { creation?.Invoke(this); }

        public Pizza_Order(DM.Pizza_Order pizza_order)
        {
            id = pizza_order.id;
            if (pizza_order.cost.HasValue) cost = pizza_order.cost.Value;
            dough_id = pizza_order.dough.id;
            pizza_id = pizza_order.pizza.id;
            quantity = pizza_order.quantity;
            size_id = pizza_order.size.id;

            if (pizza_order.order != null) order_id = pizza_order.order.id;

            creation?.Invoke(this);
        }

        public int id { get; set; }

        public int quantity
        {
            get { return m_quantity; }

            set
            {
                m_quantity = value;

                quantityChange?.Invoke();
            }
        }

        public decimal cost
        {
            get { return m_cost; }

            set
            {
                m_cost = value;

                costChange?.Invoke();
            }
        }

        public int dough_id { get; set; }

        public int size_id { get; set; }

        public int pizza_id { get; set; }

        public int order_id { get; set; }
    }
}
