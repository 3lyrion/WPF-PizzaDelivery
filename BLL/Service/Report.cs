using System;
using System.Data;
using System.Collections.Generic;
using Interfaces.Repository;
using SV = Interfaces.Service;
using DTO = Interfaces.DTO;

namespace BLL.Service
{
    public class Report : SV.IReport
    {
        IDbRepos db;

        public Report(IDbRepos repos)
        {
            db = repos;
        }

        public List<DTO.ClientOrder> GetClientsOrders()
        {
            var clients = db.Client.GetList();
            var clientsOrders = new List<DTO.ClientOrder>();

            var allPizzaOrders = db.Pizza_Order.GetList();
            var allOrders = db.Order.GetList();

            foreach (var client in clients)
            {
                var orders = allOrders.FindAll(e => e.client == client);

                var pizzaOrders = new List<DomainModel.Pizza_Order>();

                foreach (var order in orders)
                {
                    var pos = allPizzaOrders.FindAll(e => e.order == order);

                    foreach (var po in pos)
                        pizzaOrders.Add(po);
                }

                foreach (var po in pizzaOrders)
                    clientsOrders.Add(new DTO.ClientOrder
                    {
                        ClientId = client.id,
                        OrderId = po.order.id,
                        Address = po.order.address,
                        RecipientName = po.order.recipient_name,
                        DateTime = po.order.creation_date.ToString(),
                        Pizza = po.pizza.name,
                        Dough = po.dough.name,
                        Size = $"{po.size.name}, {po.size.size} см",
                        Quantity = po.quantity.ToString(),
                        Total = string.Format("{0:C2}", po.cost)
                    });
            }

            return clientsOrders;
        }
    }
}
