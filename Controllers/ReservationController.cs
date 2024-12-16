using ChatkaReservation.Data;
using ChatkaReservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ChatkaReservation.Controllers
{
    
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Zobrazení kalendáře a rezervací
        public IActionResult Calendar()
        {
            var events = _context.Reservations
                .Select(r => new CalendarEvent
                {
                    Title = r.CustomerName,
                    Start = r.StartDate.AddDays(1),
                    End = r.EndDate.AddDays(1) // FullCalendar end date is exclusive
                })
                .ToList();

            return View(new CalendarModel
            {
                CottageID = 1, // ID chatky
                Events = events
            });
        }

        // Zobrazení formuláře pro vytvoření rezervace
        public IActionResult Create()
        {
            var cottages = _context.Cottages
                .Select(c => new { c.CottageID, c.Name })
                .ToList();

            ViewBag.Cottages = new SelectList(cottages, "CottageID", "Name");

            return View();
        }

        // Akce pro vytvoření rezervace (používá AJAX pro POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateReservation([FromBody] ReservationCreateModel model)
        {
            if (ModelState.IsValid)
            {
                // Logování přijatých dat
                Console.WriteLine($"Creating reservation: CottageID={model.CottageID}, StartDate={model.StartDate}, EndDate={model.EndDate}");

                // Zkontrolování, zda je časový interval již obsazen
                var existingReservation = _context.Reservations
                    .FirstOrDefault(r => r.CottageID == model.CottageID &&
                                        ((r.StartDate < model.EndDate) && (r.EndDate > model.StartDate)));

                if (existingReservation != null)
                {
                    return BadRequest("Tento čas je již obsazen.");
                }

                // Vytvoření nové rezervace
                var reservation = new Reservation
                {
                    CottageID = model.CottageID,
                    StartDate = model.StartDate, 
                    EndDate = model.EndDate,     
                    CustomerName = model.CustomerName,
                    CustomerEmail = model.CustomerEmail,
                    Notes = model.ReservationNotes
                };

                _context.Reservations.Add(reservation);
                _context.SaveChanges();

                Console.WriteLine("Reservation created successfully.");
                return Ok(); // Vrátí odpověď na úspěšné vytvoření rezervace
            }
            // Logování neplatného ModelState
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                Console.WriteLine($"ModelState Error: {error.ErrorMessage}");
            }
            return BadRequest("Chyba při vytváření rezervace.");
        }

        // Akce pro zobrazení seznamu všech rezervací
        public IActionResult List()
        {
            var reservations = _context.Reservations
                .Include(r => r.Cottage)
                .ToList();
            return View(reservations);
        }

        // GET akce pro úpravu rezervace
        public IActionResult Edit(int id)
        {
            var reservation = _context.Reservations.Find(id);
            if (reservation == null) return NotFound();

            var editModel = new ReservationEditModel
            {
                ID = reservation.ID,
                CottageID = reservation.CottageID,
                CustomerName = reservation.CustomerName,
                CustomerEmail = reservation.CustomerEmail,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                Notes = reservation.Notes
            };

            var cottages = _context.Cottages
                .Select(c => new { c.CottageID, c.Name })
                .ToList();

            ViewBag.Cottages = new SelectList(cottages, "CottageID", "Name", reservation.CottageID);

            return View(editModel);
        }

        // POST: Reservation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [FromBody] ReservationEditModel model)
        {
            if (id != model.ID)
            {
                return BadRequest("ID se neshoduje.");
            }

            if (!ModelState.IsValid)
            {
                // Log validation errors
                var validationErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                foreach (var error in validationErrors)
                {
                    Console.WriteLine($"Validation error: {error}");
                }
                return BadRequest("Chyba při odesílání formuláře.");
            }

            var reservation = _context.Reservations.Find(id);
            if (reservation == null)
            {
                return NotFound("Rezervace nebyla nalezena.");
            }

            // Kontrola, zda je nový termín rezervace již obsazený
            var existingReservation = _context.Reservations
                .FirstOrDefault(r => r.CottageID == model.CottageID &&
                                     ((r.StartDate < model.EndDate) && (r.EndDate > model.StartDate)) &&
                                     r.ID != id);

            if (existingReservation != null)
            {
                return BadRequest("Tento čas je již obsazen.");
            }

            // Aktualizace údajů rezervace
            reservation.CustomerName = model.CustomerName;
            reservation.CustomerEmail = model.CustomerEmail;
            reservation.StartDate = model.StartDate.Date; // Nastavení času na 00:00:00
            reservation.EndDate = model.EndDate.Date;     // Nastavení času na 00:00:00
            reservation.Notes = model.Notes;
            reservation.CottageID = model.CottageID;

            // Uložení změn do databáze
            try
            {
                _context.SaveChanges();
                Console.WriteLine("Reservation updated successfully."); // Debug zpráva
                return Ok(); // Odpověď na úspěšné uložení změn
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Chyba při aktualizaci rezervace: " + ex.Message);
                return BadRequest("Chyba při aktualizaci rezervace: " + ex.Message);
            }
        }

        // Akce pro smazání rezervace
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var reservation = _context.Reservations.Find(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(List));
        }
    }
}