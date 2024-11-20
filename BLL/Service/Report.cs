using System;
using System.Data;
using Interfaces.Repository;
using SV = Interfaces.Service;

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
    }
}
