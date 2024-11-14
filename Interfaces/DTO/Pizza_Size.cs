using System;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;

namespace Interfaces.DTO
{
    public partial class Pizza_Size
    {
        public Pizza_Size() { }

        public Pizza_Size(DM.Pizza_Size pizza_size)
        {
            id = pizza_size.id;
            name = pizza_size.name;
            size = pizza_size.size;
            cost_mult = pizza_size.cost_mult;
            weight_mult = pizza_size.weight_mult;
        }

        public int id { get; set; }

        public string name { get; set; }

        public int size { get; set; }

        public decimal cost_mult { get; set; }

        public decimal weight_mult { get; set; }
    }
}
