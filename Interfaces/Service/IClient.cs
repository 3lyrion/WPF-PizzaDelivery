using System;
using System.Collections.Generic;

namespace Interfaces.Service
{
    public interface IClient
    {
        int Create(DTO.Client clientDto);

        bool Update(DTO.Client clientDto);

        bool Delete(int id);

        bool Save();

        List<DTO.Client> GetList();
    }
}
