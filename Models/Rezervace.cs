using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Přidání pro validaci
using Microsoft.AspNetCore.Identity; // Přidání pro IdentityUser

namespace ChatkaReservation.Models
{
    public class Rezervace : IValidatableObject // Implementace IValidatableObject pro vlastní validaci
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Datum od je povinné.")]
        public DateTime DatumOd { get; set; }

        [Required(ErrorMessage = "Datum do je povinné.")]
        public DateTime DatumDo { get; set; }

        [Required(ErrorMessage = "ID chatky je povinné.")]
        public int ChatkaId { get; set; }

        // Propojení s chatkou
        public required Chatka Chatka { get; set; }

        // Propojení s uživatelem
        [Required(ErrorMessage = "ID uživatele je povinné.")]
        public required string UzivatelId { get; set; } // Změna na ID uživatele
        public IdentityUser? Uzivatel { get; set; } // Propojení s uživatelským modelem

        // Implementace vlastní validace
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DatumOd >= DatumDo)
            {
                yield return new ValidationResult("Datum do musí být větší než datum od.", new[] { nameof(DatumDo) });
            }
        }
    }
}