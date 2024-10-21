using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ChatkaReservation.Models;
using Microsoft.AspNetCore.Identity;

namespace ChatkaReservation.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Chatka> Chatky { get; set; }
        public DbSet<Rezervace> Rezervace { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Další konfigurace modelu (pokud je potřeba)
        }
    }
}