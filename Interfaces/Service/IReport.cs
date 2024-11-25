using System;
using System.Data;
using System.Collections.Generic;

namespace Interfaces.Service
{
    public interface IReport
    {
        DataTable GetOnlineClientOrders();

        DataTable GetOnlineCourierOrders();

        List<DTO.ClientOrder> GetClientOrders(int clientId);
    }
}
