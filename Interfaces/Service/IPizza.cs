namespace Interfaces.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IPizza
    {
        bool createPizza(DTO.Pizza pizzaDto);
        List<DTO.Pizza> getAllPizzas();
    }
}
