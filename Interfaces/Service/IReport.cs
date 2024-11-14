namespace Interfaces.Service
{
    using System;
    using System.Data;
    using System.Collections.Generic;
    using System.Linq;

    public interface IReport
    {
        DataTable get_online_clients_orders();

        DataTable get_online_couriers_orders();
    }
}
