using System;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;

namespace Interfaces.DTO
{
    public partial class Dough
    {
        public Dough() { }

        public Dough(DM.Dough dough)
        {
            Id = dough.id;
            Name = dough.name;
            Weight = dough.weight;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Weight { get; set; }
    }
}
