using System.ComponentModel.DataAnnotations;

namespace ChatkaReservation.Models
{
    public class ReservationViewModel
    {
        [Required]
        public int CottageID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}