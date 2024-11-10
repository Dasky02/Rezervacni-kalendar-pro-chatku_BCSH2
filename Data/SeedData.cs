using ChatkaReservation.Data;
using ChatkaReservation.Models;
using Microsoft.AspNetCore.Identity;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        string[] roleNames = { "Admin", "User" };

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        var adminUser = await userManager.FindByEmailAsync("admin@example.com");
        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {
                UserName = "admin@example.com",
                Email = "admin@example.com"
            };

            var result = await userManager.CreateAsync(adminUser, "Test123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
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
