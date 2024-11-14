namespace DomainModel.Base
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    public abstract class User
    {
        protected User() { }

        public int id { get; set; }

        public bool online { get; set; } = false;

        [Required]
        [StringLength(100)]
        public string full_name { get; set; }

        [Required]
        [Column(TypeName = "nchar")]
        [StringLength(12)]
        public string phone_number { get; set; }

        [Required]
        [StringLength(50)]
        public string password { get; set; }
    }
}
