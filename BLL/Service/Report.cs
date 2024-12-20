using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
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

                    var co = new DTO.ClientOrder
                    {
                        ClientId = client.id,
                        OrderId = order.id,
                        Address = order.address,
                        RecipientName = order.recipient_name,
                        DateTime = order.creation_date.ToString()
                    };

                    if (order.cost == 0.0m)
                        co.Total = string.Format("{0:C2}", pos.Sum(e => e.cost));
                    else
                        co.Total = string.Format("{0:C2}", order.cost);

                    foreach (var po in pos)
                        co.OrderParts.Add(new DTO.OrderPart
                        {
                            Pizza = po.pizza.name,
                            Dough = po.dough.name,
                            Size = $"{po.size.name}, {po.size.size} см",
                            Quantity = po.quantity.ToString(),
                            Total = string.Format("{0:C2}", po.cost)
                        });

                    clientsOrders.Add(co);
                }
            }

            return clientsOrders;
        }
    }
}
