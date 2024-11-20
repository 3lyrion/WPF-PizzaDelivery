using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel
{
    [Table("pizza_order")]
    public class Pizza_Order
    {
        public int id { get; set; }

        [Required]
        public int quantity { get; set; }

        [Required]
        public virtual Dough dough { get; set; }

        [Required]
        public virtual Pizza_Size size { get; set; }

        [Required]
        [Column(TypeName = "smallmoney")]
        public decimal cost { get; set; }

        [Required]
        public virtual Pizza pizza { get; set; }

        [Required]
        public virtual Order order { get; set; }
    }
}
