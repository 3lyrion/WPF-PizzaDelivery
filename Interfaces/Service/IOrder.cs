using System;
using System.Collections.Generic;

namespace Interfaces.Service
{
    public interface IOrder
    {
        int Create(DTO.Order orderDto);

        bool Delete(int id);

        bool Save();

        List<DTO.Order> GetList();

        void PassOrderToCook(int orderId);

        void PassOrderToCourier(int orderId);

        void CloseOrder(int orderId, int status, bool courier = true);
    }
}
