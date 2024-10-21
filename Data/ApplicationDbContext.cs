using Microsoft.EntityFrameworkCore;
using ChatkaReservation.Models;

namespace ChatkaReservation.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Chatka> Chatky { get; set; }
        public DbSet<Rezervace> Rezervace { get; set; }
    }
}