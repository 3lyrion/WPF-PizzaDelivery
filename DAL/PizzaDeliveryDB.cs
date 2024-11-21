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
            : base("name=PizzaDeliveryDB")
        {
        }

        public DbSet<DM.Client> Client { get; set; }

        public DbSet<DM.Cook> Cook { get; set; }

        public DbSet<DM.Courier> Courier { get; set; }

        public DbSet<DM.Dough> Dough { get; set; }

        public DbSet<DM.Ingredient> Ingredient { get; set; }

        public DbSet<DM.Order> Order { get; set; }

        public DbSet<DM.Pizza> Pizza { get; set; }

        public DbSet<DM.Pizza_Order> Pizza_Order { get; set; }

        public DbSet<DM.Pizza_Size> Pizza_Size { get; set; }

        public DbSet<DM.Recipe> Recipe { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*
            modelBuilder.Entity<DM.Pizza_Size>()
                .Property(e => e.cost_mult)
                .HasPrecision(10, 4);

            modelBuilder.Entity<DM.Pizza_Size>()
                .Property(e => e.weight_mult)
                .HasPrecision(10, 4);
            
            modelBuilder.Entity<Client>()
                .Property(e => e.phone_number)
                .IsFixedLength();

            modelBuilder.Entity<Client>()
                .HasMany(e => e.client_order)
                .WithRequired(e => e.client)
                .HasForeignKey(e => e.client_id)
                .WillCascadeOnDelete(false);
            */

            /*
            modelBuilder.Entity<Order>()
                .HasMany(e => e.delivery_man)
                .WithOptional(e => e.client_order)
                .HasForeignKey(e => e.order_id);
            */

            /*
            modelBuilder.Entity<Dough_Ingredient>()
                .HasMany(e => e.dough_recipe)
                .WithRequired(e => e.dough_ingredient)
                .HasForeignKey(e => e.dough_ingredient_id)
                .WillCascadeOnDelete(false);
            */

            /*
            modelBuilder.Entity<pizza>()
                .HasMany(e => e.dough_recipe)
                .WithRequired(e => e.pizza)
                .HasForeignKey(e => e.pizza_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<pizza>()
                .HasMany(e => e.topping_recipe)
                .WithRequired(e => e.pizza)
                .HasForeignKey(e => e.pizza_id)
                .WillCascadeOnDelete(false);
            */

            /*
            modelBuilder.Entity<Topping_Ingredient>()
                .HasMany(e => e.topping_recipe)
                .WithRequired(e => e.topping_ingredient)
                .HasForeignKey(e => e.topping_ingredient_id)
                .WillCascadeOnDelete(false);
            */
        }
    }
}
