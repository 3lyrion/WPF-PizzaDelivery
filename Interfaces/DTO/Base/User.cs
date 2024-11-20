using System;
using System.Collections.Generic;

namespace Interfaces.DTO.Base
{
    public abstract class User
    {
        protected User() { }

        public int Id { get; set; }

        public bool Online { get; set; } = false;

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }
    }
}
