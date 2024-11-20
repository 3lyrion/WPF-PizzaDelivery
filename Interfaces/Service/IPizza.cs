using System;
using System.Collections.Generic;

namespace Interfaces.Service
{
    public interface IPizza
    {
        int Create(DTO.Pizza pizzaDto);

        bool Save();

        List<DTO.Pizza> GetList();
    }
}
