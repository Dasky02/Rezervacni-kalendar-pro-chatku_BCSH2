using System;

namespace ChatkaReservation.Models
{
    public class Rezervace
    {
        public int Id { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }
        public int ChatkaId { get; set; }

        // Propojení s Chatkou
        public required Chatka Chatka { get; set; }
        
        // Možné propojení s uživatelem
        public required string Uzivatel { get; set; }
    }
}