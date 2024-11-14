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
        List<DTO.OnlineClientOrders> get_online_clients_orders();

        List<DTO.OnlineCourierOrder> get_online_couriers_orders();
    }
}
