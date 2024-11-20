using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repository
{
    public interface IReportRepository
    {
        List<DTO.OnlineClientOrders> GetOnlineClientOrders();

        List<DTO.OnlineCourierOrder> GetOnlineCourierOrders();
    }
}
