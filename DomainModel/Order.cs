using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel
{
    [Table("order_")]
    public class Order
    {
        public Order()
        {
            pizza_order = new HashSet<Pizza_Order>();
        }

        public int id { get; set; }

        public DateTime creation_date { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "smallmoney")]
        public decimal cost { get; set; }

        [Required]
        [StringLength(150)]
        public string address { get; set; }

        public int status { get; set; } = 0;

        [Required]
        public virtual Client client { get; set; }

        public virtual Cook cook { get; set; }

        public virtual Courier courier { get; set; }

        public virtual ICollection<Pizza_Order> pizza_order { get; set; }
    }
}
