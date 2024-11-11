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

        // Zobrazení kalendáøe a rezervací
        public IActionResult Calendar()
        {
            var events = _context.Reservations
                .Select(r => new CalendarEvent
                {
                    Title = r.CustomerName,
                    Start = r.StartDate.Date, // Pouze datum, èas je ignorován
                    End = r.EndDate.Date // Pouze datum, èas je ignorován
                })
                .ToList();

            return View(new CalendarModel
            {
                CottageID = 1, // ID chatky
                Events = events
            });
        }


        // Zobrazení formuláøe pro vytvoøení rezervace
        public IActionResult Create()
        {
            var cottages = _context.Cottages
                .Select(c => new { c.CottageId, c.Name }) // Naèítáme ID a název chatky
                .ToList();

            ViewBag.Cottages = new SelectList(cottages, "CottageID", "Name");

            return View();
        }

        // Akce pro vytvoøení rezervace (používá AJAX pro POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateReservation([FromBody] ReservationCreateModel model)
        {
            if (ModelState.IsValid)
            {
                // Zkontrolování, zda je èasový interval již obsazen
                var existingReservation = _context.Reservations
                    .FirstOrDefault(r => r.CottageID == model.CottageID &&
                                         ((r.StartDate > model.StartDate && r.StartDate < model.EndDate) ||
                                          (r.EndDate > model.StartDate && r.EndDate <= model.EndDate)));

                if (existingReservation != null)
                {
                    return BadRequest("Tento èas je již obsazen.");
                
                }

                // Vytvoøení nové rezervace
                var reservation = new Reservation
                {
                    CottageID = model.CottageID,
                    StartDate = model.StartDate.AddDays(1),
                    EndDate = model.EndDate.AddDays(0),
                    CustomerName = model.CustomerName,
                    CustomerEmail = model.CustomerEmail,
                    Notes = model.ReservationNotes  // Uložení poznámky
                };

                _context.Reservations.Add(reservation);
                _context.SaveChanges();

                return Ok(); // Vrátí odpovìï na úspìšné vytvoøení rezervace
            }
            return BadRequest("Chyba pøi vytváøení rezervace.");
        }



        // Akce pro zobrazení seznamu všech rezervací
        public IActionResult List()
        {
            var reservations = _context.Reservations
                .Include(r => r.Cottage)  // Naètení dat z navázané tabulky Cottage
                .ToList();
            return View(reservations);
        }

        //GET akce pro úpravu rezervace
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
                Console.WriteLine("ID does not match reservation ID."); // Debug zpráva
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    _context.SaveChanges();
                    Console.WriteLine("Reservation updated successfully."); // Debug zpráva
                    return RedirectToAction(nameof(Manage));
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine("Error updating reservation: " + ex.Message); // Debug zpráva
                    ModelState.AddModelError("", "Error updating reservation: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("ModelState is not valid"); // Debug zpráva
            }
            return View(reservation);
        }


        public IActionResult Manage()
        {
            var reservations = _context.Reservations.Include(r => r.Cottage).ToList();
            return View(reservations);
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
