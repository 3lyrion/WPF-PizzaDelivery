﻿namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("recipe")]
    public class Recipe
    {
        public int id { get; set; }

        [Required]
        public int quantity { get; set; }

        [Required]
        public virtual Pizza pizza { get; set; }

        [Required]
        public Ingredient ingredient { get; set; }
    }
}