namespace Interfaces.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IOrder
    {
        int createOrder(DTO.Order orderDto);

        bool deleteOrder(int id);

        bool save();

        List<DTO.Order> getAllOrders();
    }
}
