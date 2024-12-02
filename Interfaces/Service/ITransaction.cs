using System;
using System.Data;
using System.Collections.Generic;

namespace Interfaces.Service
{
    public interface ITransaction
    {
        void PassOrderToCook(int orderId);

        void PassOrderToCourier(int OrderId);

        void CloseOrder(int id, int status);
    }
}
