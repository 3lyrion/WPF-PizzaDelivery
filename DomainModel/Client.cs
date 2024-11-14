namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("client")]
    public class Client : Base.User
    {
        public Client()
        {
            order = new HashSet<Order>();
        }

        public ICollection<Order> order { get; set; }
    }
}
