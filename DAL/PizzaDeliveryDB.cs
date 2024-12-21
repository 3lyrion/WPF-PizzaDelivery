using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using DM = DomainModel;

namespace DAL
{
    public class PizzaDeliveryDB : DbContext
    {
        public PizzaDeliveryDB()
            : base("PizzaDeliveryDB")
        {
        }

        public virtual DbSet<DM.Client> Client { get; set; }

        public virtual DbSet<DM.Cook> Cook { get; set; }

        public virtual DbSet<DM.Courier> Courier { get; set; }

        public virtual DbSet<DM.Dough> Dough { get; set; }

        public virtual DbSet<DM.Ingredient> Ingredient { get; set; }

        public virtual DbSet<DM.Order> Order { get; set; }

        public virtual DbSet<DM.Pizza> Pizza { get; set; }

        public virtual DbSet<DM.Pizza_Order> Pizza_Order { get; set; }

        public virtual DbSet<DM.Pizza_Size> Pizza_Size { get; set; }

        public virtual DbSet<DM.Recipe> Recipe { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
