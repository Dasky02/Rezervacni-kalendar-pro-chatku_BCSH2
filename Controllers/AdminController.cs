using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using ChatkaReservation.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace ChatkaReservation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AdminController> _logger;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        // GET: /Admin/Index
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        // GET: /Admin/EditRoles/userId
        public async Task<IActionResult> EditRoles(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var model = new EditRolesViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                Roles = _roleManager.Roles.Select(r => r.Name).ToList(),
                UserRoles = await _userManager.GetRolesAsync(user),
                SelectedRoles = new List<string>() // Inicializace prázdného seznamu
            };

            return View(model);
        }

        // POST: /Admin/EditRoles
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoles(EditRolesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Roles = _roleManager.Roles.Select(r => r.Name).ToList();
                model.UserRoles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(model.UserId));
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var selectedRoles = model.SelectedRoles ?? new List<string>();

            // Logování přijatých rolí
            _logger.LogInformation("SelectedRoles Count: {Count}", selectedRoles.Count);
            _logger.LogInformation("SelectedRoles: {SelectedRoles}", string.Join(", ", selectedRoles));

            // Přidání nových rolí
            var addResult = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
            if (!addResult.Succeeded)
            {
                foreach (var error in addResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    _logger.LogError("Error adding role: {Error}", error.Description);
                }
                model.Roles = _roleManager.Roles.Select(r => r.Name).ToList();
                model.UserRoles = await _userManager.GetRolesAsync(user);
                return View(model);
            }

            // Odebrání nepotřebných rolí
            var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
            if (!removeResult.Succeeded)
            {
                foreach (var error in removeResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    _logger.LogError("Error removing role: {Error}", error.Description);
                }
                model.Roles = _roleManager.Roles.Select(r => r.Name).ToList();
                model.UserRoles = await _userManager.GetRolesAsync(user);
                return View(model);
            }

            _logger.LogInformation("Roles updated for user {Email}.", user.Email);
            return RedirectToAction(nameof(Index));
        }
    }
}