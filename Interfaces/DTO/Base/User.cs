namespace Interfaces.DTO.Base
{
    using System;
    using System.Collections.Generic;

    public abstract class User
    {
        protected User() { }

        public int id { get; set; }

        public bool online { get; set; } = false;

        public string full_name { get; set; }

        public string phone_number { get; set; }

        public string password { get; set; }
    }
}
