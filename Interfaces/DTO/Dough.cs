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
            id = dough.id;
            name = dough.name;
            weight = dough.weight;
        }

        public int id { get; set; }

        public string name { get; set; }

        public int weight { get; set; }
    }
}
