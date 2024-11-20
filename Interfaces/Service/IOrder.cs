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
    }
}
