namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("order_status")]
    public class Order_Status
    {
        public int id { get; set; }

        [Required]
        [StringLength(25)]
        public string name { get; set; }
    }
}
