using System;
using System.Collections.Generic;

namespace Interfaces.Service
{
    public interface IRecipe
    {
        int Create(DTO.Recipe recipeDto);

        bool Update(DTO.Recipe recipeDto);

        bool Delete(int id);

        bool Save();

        List<DTO.Recipe> GetList();
    }
}
