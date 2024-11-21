using System;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;

namespace Interfaces.DTO
{
    public partial class PizzaSize
    {
        public PizzaSize() { }

        public PizzaSize(DM.Pizza_Size pizza_size)
        {
            Id = pizza_size.id;
            Name = pizza_size.name;
            Size = pizza_size.size;
            CostMult = pizza_size.cost_mult;
            WeightMult = pizza_size.weight_mult;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Size { get; set; }

        public double CostMult { get; set; }

        public double WeightMult { get; set; }
    }
}
