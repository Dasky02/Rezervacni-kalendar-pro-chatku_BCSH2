using System;
using System.ComponentModel.DataAnnotations;

namespace ChatkaReservation.Models
{
    public class ReservationCreateModel
    {
        [Required]
        public int CottageID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; }

        public string ReservationNotes { get; set; }
    }
}