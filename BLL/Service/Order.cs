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

        public int createOrder(DTO.Order orderDto)
        {
            var order = new DM.Order();
            var poList = new List<DM.Pizza_Order>();
            var sum = 0.0m;

            foreach (var poId in orderDto.pizza_order_IDs)
            {
                var po = db.pizza_order.getItem(poId);
                po.order = order;

                sum += po.cost.Value;

                poList.Add(po);
            }

//            var cook = db.cook.getList().First(e => e.online && !e.busy);
//            cook.busy = true;

            order.creation_date = DateTime.Now;
            order.address = orderDto.address;
            order.cost = sum;
            order.client = db.client.getItem(orderDto.client_id);
            // order.cook = cook;
            //    status = db.order_status.Single(e => e.name == "Приготовление"),
            order.pizza_order = poList;

            db.order.create(order);

            //db.cook.First(e => e.online && e.order != null).
            //    order = order;

            if (save())
                return order.id;

            return 0;
        }

        public bool deleteOrder(int id)
        {
            var cl = db.order.getItem(id);
            if (cl != null) db.order.delete(id);

            return save();
        }

        public bool save()
        {
            return db.save() > 0;
        }

        public List<DTO.Order> getAllOrders()
        {
            return db.order.getList().Select(i => new DTO.Order(i)).ToList();
        }
    }
}
