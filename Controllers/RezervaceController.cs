using System.Security.Claims;
using ChatkaReservation.Data;
using ChatkaReservation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ChatkaReservation.Controllers
{
    [Authorize] // Umožňuje přístup pouze přihlášeným uživatelům
    public class RezervaceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RezervaceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rezervace
        public async Task<IActionResult> Index()
        {
            var rezervace = await _context.Rezervace
                .Include(r => r.Chatka)
                .Include(r => r.Uzivatel)
                .ToListAsync();
            return View(rezervace);
        }

        // GET: Rezervace/Create
        public IActionResult Create()
        {
            ViewBag.Chatky = new SelectList(_context.Chatky, "Id", "Nazev");
            return View();
        }

        // POST: Rezervace/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DatumOd,DatumDo,ChatkaId,UzivatelId")] Rezervace rezervace)
        {
            if (ModelState.IsValid)
            {
                rezervace.UzivatelId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Add(rezervace);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Chatky = new SelectList(_context.Chatky, "Id", "Nazev", rezervace.ChatkaId);
            return View(rezervace);
        }

        // GET: Rezervace/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var rezervace = await _context.Rezervace.FindAsync(id);
            if (rezervace == null)
            {
                return NotFound();
            }
            ViewBag.Chatky = new SelectList(_context.Chatky, "Id", "Nazev", rezervace.ChatkaId);
            return View(rezervace);
        }

        // POST: Rezervace/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DatumOd,DatumDo,ChatkaId,UzivatelId")] Rezervace rezervace)
        {
            if (id != rezervace.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rezervace);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RezervaceExists(rezervace.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Chatky = new SelectList(_context.Chatky, "Id", "Nazev", rezervace.ChatkaId);
            return View(rezervace);
        }

        private bool RezervaceExists(int id)
        {
            return _context.Rezervace.Any(e => e.Id == id);
        }
    }
}