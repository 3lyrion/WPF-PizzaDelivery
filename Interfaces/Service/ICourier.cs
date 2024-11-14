namespace Interfaces.Service
{
    using System;
    using System.Data;
    using System.Collections.Generic;
    using System.Linq;

    public interface ICourier
    {
        bool save();

        List<DTO.Courier> getAllCouriers();
    }
}
