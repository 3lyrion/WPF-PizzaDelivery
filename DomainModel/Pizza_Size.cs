using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel
{
    [Table("pizza_size")]
    public class Pizza_Size
    {
        public int id { get; set; }

        [Required]
        [StringLength(25)]
        public string name { get; set; }

        [Required]
        public int size { get; set; }

        [Required]
        public double cost_mult { get; set; }

        [Required]
        public double weight_mult { get; set; }
    }
}
