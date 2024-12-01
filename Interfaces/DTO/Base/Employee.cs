using System;
using System.Collections.Generic;

namespace Interfaces.DTO.Base
{
    public abstract class Employee : User
    {
        protected Employee() { }

        public string FullName { get; set; }

        public bool Busy { get; set; } = false;
    }
}
