using ChatkaReservation.Data;
using ChatkaReservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

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
                                         ((r.StartDate > model.StartDate && r.StartDate < model.EndDate) ||
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
                    EndDate = model.EndDate.AddDays(0),
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



        // Akce pro zobrazen� seznamu v�ech rezervac�
        public IActionResult List()
        {
            var reservations = _context.Reservations
                .Include(r => r.Cottage)  // Na�ten� dat z nav�zan� tabulky Cottage
                .ToList();
            return View(reservations);
        }

        //GET akce pro �pravu rezervace
        public IActionResult Edit(int id)
        {
            var reservation = _context.Reservations.Find(id);
            if (reservation == null) return NotFound();
            return View(reservation);
        }

        // POST: Reservation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Reservation reservation)
        {
            if (id != reservation.ID)
            {
                Console.WriteLine("ID does not match reservation ID."); // Debug zpr�va
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    _context.SaveChanges();
                    Console.WriteLine("Reservation updated successfully."); // Debug zpr�va
                    return RedirectToAction(nameof(Manage));
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine("Error updating reservation: " + ex.Message); // Debug zpr�va
                    ModelState.AddModelError("", "Error updating reservation: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("ModelState is not valid"); // Debug zpr�va
            }
            return View(reservation);
        }


        public IActionResult Manage()
        {
            var reservations = _context.Reservations.Include(r => r.Cottage).ToList();
            return View(reservations);
        }





        // Akce pro smaz�n� rezervace
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
