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

        public DataTable GetOnlineClientOrders()
        {
            var table = new DataTable();
            table.Columns.Add("ID");
            table.Columns.Add("ФИО");
            table.Columns.Add("Заказы (ID)");

            var res = db.Report.GetOnlineClientOrders();
            foreach (var oco in res)
            {
                var row = table.NewRow();
                row["ID"] = oco.Id;
                row["ФИО"] = oco.FullName;
                row["Заказы (ID)"] = oco.OrdersIds;

                table.Rows.Add(row);
            }

            return table;
        }

        public DataTable GetOnlineCourierOrders()
        {
            var table = new DataTable();
            table.Columns.Add("ID");
            table.Columns.Add("ФИО");
            table.Columns.Add("Заказ (ID)");

            var res = db.Report.GetOnlineCourierOrders();
            foreach (var oco in res)
            {
                var row = table.NewRow();
                row["ID"] = oco.Id;
                row["ФИО"] = oco.FullName;
                row["Заказ (ID)"] = oco.OrderId;

                table.Rows.Add(row);
            }

            return table;
        }

        public List<DTO.ClientOrder> GetClientOrders(int clientId)
        {
            var client = db.Client.GetItem(clientId);

            var orders = db.Order.GetList().FindAll(e => e.client == client);

            var pizzaOrders = new List<DomainModel.Pizza_Order>();

            var allPizzaOrders = db.Pizza_Order.GetList();

            foreach (var order in orders)
            {
                var pos = allPizzaOrders.FindAll(e => e.order == order);

                foreach (var po in pos)
                    pizzaOrders.Add(po);
            }
            /*
            var table = new DataTable();
            table.Columns.Add("Пицца");
            table.Columns.Add("Тесто");
            table.Columns.Add("Размер");
            table.Columns.Add("Количество");
            table.Columns.Add("Итог");

            foreach (var po in pizzaOrders)
            {
                var row = table.NewRow();
                row["Пицца"] = po.pizza.name;
                row["Тесто"] = po.dough.name;
                row["Размер"] = $"{po.size.name}, {po.size.size} см";
                row["Количество"] = po.quantity;
                row["Итог"] = string.Format("{0:C2}", po.cost * (decimal)po.size.weight_mult * po.quantity);

                table.Rows.Add(row);
            }

            return table;
            */

            var clientOrders = new List<DTO.ClientOrder>();

            foreach (var po in pizzaOrders)
                clientOrders.Add(new DTO.ClientOrder
                {
                    OrderId = po.order.id,
                    DateTime = po.order.creation_date.ToString(),
                    Pizza = po.pizza.name,
                    Dough = po.dough.name,
                    Size = $"{po.size.name}, {po.size.size} см",
                    Quantity = po.quantity.ToString(),
                    Total = string.Format("{0:C2}", po.cost)
                });

            return clientOrders;
        }
    }
}
