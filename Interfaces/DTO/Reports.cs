﻿using System;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel;

namespace Interfaces.DTO
{
    public class ClientOrder
    {
        public int ClientId { get; set; }

        public int OrderId { get; set; }

        public string Address { get; set; }

        public string RecipientName { get; set; }

        public string DateTime { get; set; }

        public string Pizza { get; set; }

        public string Dough { get; set; }

        public string Size { get; set; }

        public string Quantity { get; set; }

        public string Total { get; set; }
    }
}
