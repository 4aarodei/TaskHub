using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskHub.Controllers;
using TaskHub.Data;
using TaskHub.Models;
using TaskHub.Services;
using TaskHub.Services.PlayListServices;
using TaskHub.Models.Playlist.NewBackGroungLogic; // Додаємо using для Hubs
using TaskHub.Workers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// Add custom services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PlaylistService>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<TeamService>();
builder.Services.AddScoped<InviteService>();
builder.Services.AddHttpContextAccessor();

// playList (Твої існуючі сервіси для плейлистів)
builder.Services.AddScoped<IWS_Service, FakeWS_Service>();
// builder.Services.AddScoped<PlaylistService>(); // Закоментовано, якщо не використовується

// --- Додаємо нові сервіси для фонової генерації та SignalR ---
// Реєстрація черги задач як синглтона
builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
// Реєстрація фонового сервісу, який обробляє чергу
builder.Services.AddHostedService<PlaylistWorker>();
// Реєстрація сервісу генерації плейлистів (Scoped, оскільки він має залежність від IHubContext)
builder.Services.AddScoped<IPlaylistGeneratorService, PlaylistGeneratorService>();
// Додаємо SignalR

builder.Services.AddSignalR();
// --- Кінець додавання нових сервісів ---

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// --- Мапуємо SignalR хаб ---
app.MapHub<ProgressHub>("/progressHub");
// --- Кінець мапування SignalR хабу ---

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();