using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ChatkaReservation.Models;
using Microsoft.AspNetCore.Identity;

namespace ChatkaReservation.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet pro chatky a rezervace
        public DbSet<Cottage> Cottages { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Cottage)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.CottageID)
                .OnDelete(DeleteBehavior.Restrict); // Nebo jin� vhodn� volba
        }
    }
}
