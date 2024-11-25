using System;
using System.Collections.Generic;

namespace Interfaces.Service
{
    public interface ICook
    {
        int Create(DTO.Cook cookDto);

        bool Update(DTO.Cook cookDto);

        bool Delete(int id);

        bool Save();

        List<DTO.Cook> GetList();
    }
}
