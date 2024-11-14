namespace Interfaces.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IPizza_Order
    {
        int createPizzaOrder(DTO.Pizza_Order pizzaOrderDto);

        List<DTO.Pizza_Order> getAllPO();
    }
}
