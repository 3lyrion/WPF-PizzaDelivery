using System;
using System.Data;

namespace Interfaces.Service
{
    public interface IReport
    {
        DataTable GetOnlineClientOrders();

        DataTable GetOnlineCourierOrders();
    }
}
