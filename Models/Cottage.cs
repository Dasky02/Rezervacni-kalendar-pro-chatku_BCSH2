using System.Collections.Generic;

namespace ChatkaReservation.Models
{
    public class Cottage
    {
        public int CottageID { get; set; }
        public string Name { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }

}