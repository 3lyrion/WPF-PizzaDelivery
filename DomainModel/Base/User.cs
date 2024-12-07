using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Base
{
    public abstract class User
    {
        protected User()
        {
            order = new HashSet<Order>();
        }

        public int id { get; set; }

        public bool online { get; set; } = false;

        [Required]
        [Column(TypeName = "nchar")]
        [StringLength(12)]
        public string phone_number { get; set; }

        [Required]
        [StringLength(50)]
        public string password { get; set; }

        public virtual ICollection<Order> order { get; set; }
    }
}
