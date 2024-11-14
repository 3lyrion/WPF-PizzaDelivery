using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Interfaces.Repository;
using DTO = Interfaces.DTO;
using SV = Interfaces.Service;
using DM = DomainModel;

namespace BLL.Service
{
    public class Pizza_Order : SV.IPizza_Order
    {
        IDbRepos db;

        public Pizza_Order(IDbRepos database)
        {
            db = database;
        }

        public int createPizzaOrder(DTO.Pizza_Order pizzaOrderDto)
        {
            pizzaOrderDto.cost = db.pizza.getList().Where(
                e => e.id == pizzaOrderDto.pizza_id)
                .Sum(e => e.cost) * db.pizza_size.getItem(pizzaOrderDto.size_id).cost_mult;

            var po = db.pizza_order.getItem(db.pizza_order.create(new DM.Pizza_Order
            {
                cost = pizzaOrderDto.cost,
                dough = db.dough.getItem(pizzaOrderDto.dough_id),
                pizza = db.pizza.getItem(pizzaOrderDto.pizza_id),
                quantity = pizzaOrderDto.quantity,
                size = db.pizza_size.getItem(pizzaOrderDto.size_id)
            }));

            if (pizzaOrderDto.order_id != 0)
                po.order = db.order.getItem(pizzaOrderDto.order_id);

            if (db.save() > 0)
                return po.id;

            return 0;
        }


        public List<DTO.Pizza_Order> getAllPO()
        {
            return db.pizza_order.getList().Select(i => new DTO.Pizza_Order(i)).ToList();
        }
    }
}
