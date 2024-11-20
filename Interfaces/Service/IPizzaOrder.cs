using System;
using System.Collections.Generic;

namespace Interfaces.Service
{
    public interface IPizzaOrder
    {
        int Create(DTO.PizzaOrder pizzaOrderDto);

        bool Save();

        List<DTO.PizzaOrder> GetList();
    }
}
