using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel
{
    [Table("dough")]
    public class Dough
    {
        public Dough()
        {
            pizza_order = new HashSet<Pizza_Order>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(25)]
        public string name { get; set; }

        [Required]
        public int weight { get; set; }

        public virtual ICollection<Pizza_Order> pizza_order { get; set; }
    }
}
