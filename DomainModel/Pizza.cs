namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("pizza")]
    public class Pizza
    {
        public Pizza()
        {
            recipe = new HashSet<Recipe>();
            pizza_order = new HashSet<Pizza_Order>();
        }

        public int id { get; set; }

        public bool custom { get; set; } = false;

        public bool sales_hit { get; set; } = false;

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        [Required]
        [Column(TypeName = "smallmoney")]
        public decimal cost { get; set; }

        public int? weight { get; set; }

        public virtual ICollection<Recipe> recipe { get; set; }

        public virtual ICollection<Pizza_Order> pizza_order { get; set; }
    }
}
