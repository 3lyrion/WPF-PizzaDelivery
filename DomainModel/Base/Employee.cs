using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Base
{
    public abstract class Employee : User
    {
        protected Employee() { }

        [Required]
        [StringLength(100)]
        public string full_name { get; set; }

        public bool busy { get; set; } = false;
    }
}
