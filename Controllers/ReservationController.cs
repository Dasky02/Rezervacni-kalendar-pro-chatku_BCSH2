using ChatkaReservation.Data;
using ChatkaReservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System;

namespace ChatkaReservation.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Zobrazen� kalend��e a rezervac�
        public IActionResult Calendar()
        {
            var events = _context.Reservations
                .Select(r => new CalendarEvent
                {
                    Title = r.CustomerName,
                    Start = r.StartDate.Date, // Pouze datum, �as je ignorov�n
                    End = r.EndDate.Date // Pouze datum, �as je ignorov�n
                })
                .ToList();

            return View(new CalendarModel
            {
                CottageID = 1, // ID chatky
                Events = events
            });
        }


        // Zobrazen� formul��e pro vytvo�en� rezervace
        public IActionResult Create()
        {
            var cottages = _context.Cottages
                .Select(c => new { c.CottageId, c.Name }) // Na��t�me ID a n�zev chatky
                .ToList();

            ViewBag.Cottages = new SelectList(cottages, "CottageID", "Name");

            return View();
        }

        // Akce pro vytvo�en� rezervace (pou��v� AJAX pro POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateReservation([FromBody] ReservationCreateModel model)
        {
            if (ModelState.IsValid)
            {
                // Zkontrolov�n�, zda je �asov� interval ji� obsazen
                var existingReservation = _context.Reservations
                    .FirstOrDefault(r => r.CottageID == model.CottageID &&
                                         ((r.StartDate >= model.StartDate && r.StartDate < model.EndDate) ||
                                          (r.EndDate > model.StartDate && r.EndDate <= model.EndDate)));

                if (existingReservation != null)
                {
                    return BadRequest("Tento �as je ji� obsazen.");
                
                }

                // Vytvo�en� nov� rezervace
                var reservation = new Reservation
                {
                    CottageID = model.CottageID,
                    StartDate = model.StartDate.AddDays(1),
                    EndDate = model.EndDate.AddDays(1),
                    CustomerName = model.CustomerName,
                    CustomerEmail = model.CustomerEmail,
                    Notes = model.ReservationNotes  // Ulo�en� pozn�mky
                };

                _context.Reservations.Add(reservation);
                _context.SaveChanges();

                return Ok(); // Vr�t� odpov�� na �sp�n� vytvo�en� rezervace
            }
            return BadRequest("Chyba p�i vytv��en� rezervace.");
        }
    }
}
