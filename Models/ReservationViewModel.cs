namespace ChatkaReservation.Models
{
    public class ReservationViewModel
    {
        public required string UzivatelId { get; set; }  // Změněno na ID uživatele pro lepší vazbu
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }
        public int ChatkaId { get; set; }
    }
}