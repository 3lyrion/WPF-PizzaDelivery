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
    public class Order : SV.IOrder
    {
        IDbRepos db;

        public Order(IDbRepos database)
        {
            db = database;
        }

        public int Create(DTO.Order orderDto)
        {
            var order = new DM.Order();
            order.pizza_order = new List<DM.Pizza_Order>();
            var sum = 0.0m;

            foreach (var poId in orderDto.PizzaOrdersIds)
            {
                var po = db.Pizza_Order.GetItem(poId);
                po.order = order;

                sum += po.cost;

                order.pizza_order.Add(po);
            }

            order.address = orderDto.Address;
            order.recipient_name = orderDto.RecipientName;
            order.client = db.Client.GetItem(orderDto.ClientId);
            if (orderDto.Cost != 0) order.cost = orderDto.Cost;
            else order.cost = sum;

            order.id = db.Order.Create(order);

            db.Transaction.PassOrderToCook(order.id);

            if (Save())
                return order.id;

            return 0;
        }

        public bool Delete(int id)
        {
            var cl = db.Order.GetItem(id);
            if (cl != null) db.Order.Delete(id);

            return Save();
        }

        public bool Save()
        {
            return db.Save() > 0;
        }

        public List<DTO.Order> GetList()
        {
            return db.Order.GetList().Select(i => new DTO.Order(i)).ToList();
        }
    }
}
