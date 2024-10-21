using System.Collections.Generic;

namespace ChatkaReservation.Models
{
    public class Chatka
    {
        public int Id { get; set; }
        public required string Nazev { get; set; }
        // Přidej další vlastnosti, které chceš mít pro chatku

        // Propojení s rezervacemi (volitelné, pokud chceš mít možnost přístupu k rezervacím z chatky)
        public virtual ICollection<Rezervace> Rezervace { get; set; } = new List<Rezervace>();
    }
}