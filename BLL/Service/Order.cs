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

            db.Order.Create(order);

            if (Save())
            {
                PassOrderToCook(order.id);

                return order.id;
            }

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

        public void PassOrderToCook(int orderId)
        {
            var cooks = db.Cook.GetList().Where(e => e.online && !e.busy).ToList();
            if (cooks.Count == 0) return;

            var cook = cooks[new Random().Next(cooks.Count)];
            cook.busy = true;
            db.Cook.Update(cook);

            var order = db.Order.GetItem(orderId);
            order.status = 1;
            order.cook = cook;
            db.Order.Update(order);

            db.Save();
        }

        public void PassOrderToCourier(int orderId)
        {
            var couriers = db.Courier.GetList().Where(e => e.online && !e.busy).ToList();
            if (couriers.Count == 0) return;

            var courier = couriers[new Random().Next(couriers.Count)];
            courier.busy = true;
            db.Courier.Update(courier);

            var order = db.Order.GetItem(orderId);
            order.status = 2;
            order.courier = courier;
            db.Order.Update(order);

            var cook = order.cook;
            cook.busy = false;
            db.Cook.Update(cook);

            db.Save();

            //db.Transaction.PassOrderToCourier(orderId);
        }

        public void CloseOrder(int orderId, int status, bool courier = true)
        {
            var order = db.Order.GetItem(orderId);
            order.status = status;
            db.Order.Update(order);

            if (courier)
            {
                var courier_ = order.courier;
                courier_.busy = false;
                db.Courier.Update(courier_);
            }

            else
            {
                var cook = order.cook;
                cook.busy = false;
                db.Cook.Update(cook);
            }

            db.Save();
        }
    }
}
