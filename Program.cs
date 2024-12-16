using ChatkaReservation.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Přidání DbContext s SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Přidání Identity služeb s podporou rolí a přizpůsobenými možnostmi
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    // Nastavení politiky pro hesla
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;

    // Ostatní nastavení
    options.SignIn.RequireConfirmedAccount = false; // Vypni vyžadování potvrzení účtu
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Přidání podporovaných služeb pro MVC a Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Přidání vlastní politiky autorizace pro AdminOnly
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

var app = builder.Build();

// Inicializace rolí a administrátora
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<ApplicationDbContext>();

    // Povolujeme cizí klíče pro SQLite
    await dbContext.Database.OpenConnectionAsync();
    await dbContext.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys = ON;");

    // Inicializace seed dat (rolí, administrátora, chatky atd.)
    await SeedData.Initialize(services);
}

// Konfigurace HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Přidání middleware pro autentizaci a autorizaci
app.UseAuthentication();
app.UseAuthorization();

// Mapování výchozí trasy pro MVC a Razor Pages
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages(); // Potřebné pro Identity

app.Run(); 