using System;
using System.Collections.Generic;

namespace Interfaces.Service
{
    public interface IClient
    {
        int Create(DTO.Client clientDto);

        List<DTO.Client> GetList();

        bool Delete(int id);

        bool Save();
    }
}
