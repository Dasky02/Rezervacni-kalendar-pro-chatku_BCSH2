using Microsoft.AspNetCore.Identity;

namespace ChatkaReservation.Models
{
    public class ApplicationUser : IdentityUser
    {
        public required string Name { get; set; }
        public virtual ICollection<Reservation>? Reservations { get; set; }
    }
}
