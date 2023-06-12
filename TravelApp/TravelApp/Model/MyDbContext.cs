using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Model
{
    public class MyDbContext : DbContext
    {
        public DbSet<Restoran> Restaurants { get; set; }
        public DbSet<Atrakcija> Attractions { get; set; }
        public DbSet<Smestaj> Hotels { get; set; }
        public DbSet<Aranzman> Arrangements { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Kupovina> Bookings { get; set; }
        public DbSet<Rezervacija> Reservations { get; set; }

    }
}
