namespace Interfaces.Service
{
    using System;
    using System.Data;
    using System.Collections.Generic;
    using System.Linq;

    public interface IClient
    {
        bool createClient(DTO.Client clientDto);

        List<DTO.Client> getAllClients();

        bool deleteClient(int id);

        bool save();
    }
}
