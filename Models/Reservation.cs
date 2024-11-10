using System.ComponentModel.DataAnnotations;

namespace ChatkaReservation.Models
{
    public class Reservation
    {
        public int ID { get; set; }
        public int CottageID { get; set; }  // Cizí klíč na tabulku Cottages
        public string CustomerEmail { get; set; }
        public string CustomerName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Notes { get; set; }

        public Cottage Cottage { get; set; }  // Navigační vlastnost pro vztah
    }


}
