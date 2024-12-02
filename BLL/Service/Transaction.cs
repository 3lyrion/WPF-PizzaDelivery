using System;
using System.Data;
using System.Collections.Generic;
using Interfaces.Repository;
using SV = Interfaces.Service;

namespace BLL.Service
{
    public class Transaction : SV.ITransaction
    {
        IDbRepos db;

        public Transaction(IDbRepos theDB)
        {
            db = theDB;
        }

        public void PassOrderToCook(int orderId)
        {
            db.Transaction.PassOrderToCook(orderId);
        }

        public void PassOrderToCourier(int orderId)
        {
            db.Transaction.PassOrderToCourier(orderId);
        }

        public void CloseOrder(int id, int status)
        {
            db.Transaction.CloseOrder(id, status);
        }
    }
}
