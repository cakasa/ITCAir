using ITCAir.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNet.Identity.EntityFramework
using System;

namespace ITCAir.Data
{
    public class ITCAirContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Flight> Flights { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flight>()
                .HasMany(f => f.Reservations)
                .WithOne(r => r.Flight)
                .HasForeignKey(r => r.FlightId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ITCAirDb;Trusted_Connection=True");
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
