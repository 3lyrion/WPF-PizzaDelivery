using System;
using System.Collections.Generic;

namespace Interfaces.Service
{
    public interface IIngredient
    {
        List<DTO.Ingredient> GetList();
    }
}
