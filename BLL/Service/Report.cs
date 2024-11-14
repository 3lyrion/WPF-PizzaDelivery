using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Interfaces.Repository;
using DTO = Interfaces.DTO;
using SV = Interfaces.Service;
using DM = DomainModel;

namespace BLL.Service
{
    public class Report : SV.IReport
    {
        IDbRepos db;

        public Report(IDbRepos repos)
        {
            db = repos;
        }

        public DataTable get_online_clients_orders()
        {
            var table = new DataTable();
            table.Columns.Add("ID");
            table.Columns.Add("ФИО");
            table.Columns.Add("Заказы (ID)");

            var res = db.report.get_online_clients_orders();
            foreach (var oco in res)
            {
                var row = table.NewRow();
                row["ID"] = oco.id;
                row["ФИО"] = oco.full_name;
                row["Заказы (ID)"] = oco.order_ids;

                table.Rows.Add(row);
            }

            return table;
        }

        public DataTable get_online_couriers_orders()
        {
            var table = new DataTable();
            table.Columns.Add("ID");
            table.Columns.Add("ФИО");
            table.Columns.Add("Заказ (ID)");

            var res = db.report.get_online_couriers_orders();
            foreach (var oco in res)
            {
                var row = table.NewRow();
                row["ID"] = oco.id;
                row["ФИО"] = oco.full_name;
                row["Заказ (ID)"] = oco.order_id;

                table.Rows.Add(row);
            }

            return table;
        }
    }
}
