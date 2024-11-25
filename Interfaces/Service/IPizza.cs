using System;
using System.Collections.Generic;

namespace Interfaces.Service
{
    public interface IPizza
    {
        int Create(DTO.Pizza pizzaDto);

        bool Update(DTO.Pizza pizzaDto);

        bool Delete(int id);

        bool Save();

        List<DTO.Pizza> GetList();

        List<DTO.Recipe> GetRecipes();
    }
}
