using System.Collections.Generic;

namespace ChatkaReservation.Models
{
    public class Chatka
    {
        public int Id { get; set; }
        public required string Nazev { get; set; }
        public int Kapacita { get; set; }

        // Možná propojení s rezervacemi
        public required List<Rezervace> Rezervace { get; set; }
    }
}