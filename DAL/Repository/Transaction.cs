using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using R = Interfaces.Repository;
using DM = DomainModel;

namespace DAL.Repository
{
    public class Transaction : R.ITransactionRepository
    {
        PizzaDeliveryDB db;

        public Transaction(PizzaDeliveryDB theDB)
        {
            db = theDB;
        }
        /*
        public void PassOrderToCook(int orderId)
        {
            db.Database.ExecuteSqlCommand("pass_order_to_cook @order_id",
                new[]
                {
                    new SqlParameter("@order_id", orderId)
                }
            );
        }

        public void PassOrderToCourier(int orderId)
        {
            db.Database.ExecuteSqlCommand("pass_order_to_courier @order_id",
                new []
                {
                    new SqlParameter("@order_id", orderId)
                }
            );
        }

        public void CloseOrder(int id, int status)
        {
            db.Database.ExecuteSqlCommand("close_order @order_id, @status",
                new []
                {
                    new SqlParameter("@order_id", id),
                    new SqlParameter("@status", status)
                }
            );
        }
        */
        public void PassOrderToCook(int orderId)
        {
            db.Database.SqlQuery<object>("EXEC pass_order_to_cook @order_id",
                new[]
                {
                    new SqlParameter("@order_id", orderId)
                }
            ).ToList();
        }

        public void PassOrderToCourier(int orderId)
        {
            db.Database.SqlQuery<object>("EXEC pass_order_to_courier @order_id",
                new[]
                {
                    new SqlParameter("@order_id", orderId)
                }
            ).ToList();
        }

        public void CloseOrder(int id, int status)
        {
            db.Database.SqlQuery<object>("EXEC close_order @order_id, @status",
                new[]
                {
                    new SqlParameter("@order_id", id),
                    new SqlParameter("@status", status)
                }
            ).ToList();
        }
    }
}
