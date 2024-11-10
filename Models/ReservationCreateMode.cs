using System.ComponentModel.DataAnnotations;

namespace ChatkaReservation.Models
{
    public class ReservationCreateModel
    {
        public int CottageID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string ReservationNotes { get; set; }  // Poznámka k rezervaci
    }

}
