using System;
using System.Collections.Generic;
using System.Linq;
using DTO = Interfaces.DTO;
using R = Interfaces.Repository;

namespace DAL.Repository
{
    public class Report : R.IReportRepository
    {
        PizzaDeliveryDB db;

        public Report(PizzaDeliveryDB theDB)
        {
            db = theDB;
        }

        public List<DTO.OnlineClientOrders> GetOnlineClientOrders()
        {
            return db.Database.SqlQuery<DTO.OnlineClientOrders>
                ("SELECT * FROM dbo.get_online_clients_orders()")
                .ToList();
        }

        public List<DTO.OnlineCourierOrder> GetOnlineCourierOrders()
        {
            return db.Database.SqlQuery<DTO.OnlineCourierOrder>
                ("SELECT * FROM dbo.get_online_couriers_orders()")
                .ToList();
        }
    }
}
