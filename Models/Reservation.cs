using System.ComponentModel.DataAnnotations;

namespace ChatkaReservation.Models
{
    public class Reservation
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Cottage ID is required.")]
        public int CottageID { get; set; }  // Cizí klíč na tabulku Cottages

        [Required(ErrorMessage = "Customer Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "Customer Name is required.")]
        public string CustomerName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public string Notes { get; set; }

        public Cottage Cottage { get; set; }
    }



}
