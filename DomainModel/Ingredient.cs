namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ingredient")]
    public class Ingredient
    {
        public Ingredient()
        {
            recipe = new HashSet<Recipe>();
        }

        public bool hidden_from_client { get; set; } = false;

        public bool in_stock { get; set; } = false;

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        [Required]
        [Column(TypeName = "smallmoney")]
        public decimal cost { get; set; }


        public ICollection<Recipe> recipe;
    }
}
