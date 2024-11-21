using System;
using System.Collections.Generic;

namespace Interfaces.Repository
{
    public interface IReportRepository
    {
        List<DTO.OnlineClientOrders> GetOnlineClientOrders();

        List<DTO.OnlineCourierOrder> GetOnlineCourierOrders();
    }
}
