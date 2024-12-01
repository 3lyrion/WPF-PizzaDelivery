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
    public class PizzaOrder : SV.IPizzaOrder
    {
        IDbRepos db;

        public PizzaOrder(IDbRepos database)
        {
            db = database;
        }

        public int Create(DTO.PizzaOrder pizzaOrderDto)
        {
            if (pizzaOrderDto.Cost == 0)
                pizzaOrderDto.Cost = db.Pizza.GetList()
                    .Where(e => e.id == pizzaOrderDto.PizzaId)
                    .Sum(e => e.cost) * (decimal)db.Pizza_Size.GetItem(pizzaOrderDto.SizeId).cost_mult * pizzaOrderDto.Quantity;

            var po = new DM.Pizza_Order
            {
                cost = pizzaOrderDto.Cost,
                dough = db.Dough.GetItem(pizzaOrderDto.DoughId),
                pizza = db.Pizza.GetItem(pizzaOrderDto.PizzaId),
                quantity = pizzaOrderDto.Quantity,
                size = db.Pizza_Size.GetItem(pizzaOrderDto.SizeId)
            };

            if (pizzaOrderDto.OrderId.HasValue)
                po.order = db.Order.GetItem(pizzaOrderDto.OrderId.Value);

            po.id = db.Pizza_Order.Create(po);

            if (Save())
                return po.id;

            return 0;
        }

        public bool Save()
        {
            return db.Save() > 0;
        }

        public List<DTO.PizzaOrder> GetList()
        {
            return db.Pizza_Order.GetList().Select(i => new DTO.PizzaOrder(i)).ToList();
        }
    }
}
