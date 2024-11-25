using System;
using System.Collections.Generic;

namespace Interfaces.Service
{
    public interface ICourier
    {
        int Create(DTO.Courier courierDto);

        bool Update(DTO.Courier courierDto);

        bool Delete(int id);

        bool Save();

        List<DTO.Courier> GetList();
    }
}
