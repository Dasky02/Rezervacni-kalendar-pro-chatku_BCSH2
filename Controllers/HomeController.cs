// HomeController.cs - odstranění akcí registrace a přihlášení
using System.Diagnostics;
using ChatkaReservation.Data;
using ChatkaReservation.Models;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context; // Přidání DbContext

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context; // Inicializace DbContext
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Reservation()
    {
        return View();
    }

    public IActionResult Availability()
    {
        var reservations = _context.Rezervace.ToList(); // Načtení rezervací z databáze
        return View(reservations); // Předání rezervací do pohledu
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}