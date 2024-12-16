using ChatkaReservation.Data;
using ChatkaReservation.Models;
using Microsoft.AspNetCore.Identity;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        // Inicializace rolí
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        string[] roles = { "Admin", "User" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            { 
               await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Inicializace administrátorského uživatele
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        string adminEmail = "admin@chatkarezervace.cz";
        string adminPassword = "Admin@123"; // Ujistěte se, že heslo splňuje požadavky Identity

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
            else
            {
                throw new Exception("Failed to create admin user.");
            }
        }

        var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

        // Přidání seed dat pro chatky, pokud nejsou přítomna
        if (!dbContext.Cottages.Any())
        {
            dbContext.Cottages.Add(new Cottage { Name = "Chatka A" });
            await dbContext.SaveChangesAsync();
        }
    }
} 