using System.Collections.Generic;

namespace ChatkaReservation.Models
{
    public class Cottage
    {
        public int CottageId { get; set; }
        public string Name { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }

}