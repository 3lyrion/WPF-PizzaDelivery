using System;
using System.Collections.Generic;

namespace Interfaces.Service
{
    public interface ICourier
    {
        List<DTO.Courier> GetList();
    }
}
