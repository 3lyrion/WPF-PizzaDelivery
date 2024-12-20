using System;
using System.Data;
using System.Collections.Generic;

namespace Interfaces.Service
{
    public interface IReport
    {
        List<DTO.ClientOrder> GetClientsOrders();
    }
}
