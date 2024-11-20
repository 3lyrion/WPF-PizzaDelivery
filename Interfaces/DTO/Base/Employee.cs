using System;
using System.Collections.Generic;

namespace Interfaces.DTO.Base
{
    public abstract class Employee : User
    {
        protected Employee() { }

        public bool Busy { get; set; } = false;
    }
}
