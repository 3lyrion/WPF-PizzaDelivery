namespace DomainModel.Base
{
    using System;
    using System.Collections.Generic;

    public abstract class Employee : User
    {
        protected Employee() { }

        public bool busy { get; set; } = false;
    }
}
