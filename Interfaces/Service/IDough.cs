namespace Interfaces.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IDough
    {
        List<DTO.Dough> getAllDough();
    }
}
