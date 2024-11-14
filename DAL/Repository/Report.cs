namespace DAL.Repository
{
    using System;
    using System.Data;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Linq;
    using DM = DomainModel;
    using R = Interfaces.Repository;
    using DTO = Interfaces.DTO;

    public class Report : R.IReportRepository
    {
        PizzaDeliveryDB db;

        public Report(PizzaDeliveryDB theDB)
        {
            db = theDB;
        }

        public List<DTO.OnlineClientOrders> get_online_clients_orders()
        {
            return db.Database.SqlQuery<DTO.OnlineClientOrders>
                ("SELECT * FROM dbo.get_online_clients_orders()")
                .ToList();
        }

        public List<DTO.OnlineCourierOrder> get_online_couriers_orders()
        {
            return db.Database.SqlQuery<DTO.OnlineCourierOrder>
                ("SELECT * FROM dbo.get_online_couriers_orders()")
                .ToList();
        }
    }
}
