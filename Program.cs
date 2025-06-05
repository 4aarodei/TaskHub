using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskHub.Controllers;
using TaskHub.Data;
using TaskHub.Hubs;
using TaskHub.Models;
using TaskHub.Services;
using TaskHub.Services.PlayListServices;
using TaskHub.PlaylistWorker;

var builder = WebApplication.CreateBuilder(args);

// ===========================
// 🔌 Database Configuration
// ===========================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// ===========================
// 👤 Identity Configuration
// ===========================
builder.Services.AddDefaultIdentity<AppUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// ===========================
// ⚙ MVC & Razor Pages
// ===========================
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// ===========================
// 🧩 Application Services
// ===========================
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PlaylistService>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<TeamService>();
builder.Services.AddScoped<InviteService>();
builder.Services.AddHttpContextAccessor();

// ===========================
// 🧠 Плейліст-сервіси (WS/реальні/фейкові)
// ===========================
builder.Services.AddScoped<IWS_Service, FakeWS_Service>();
// Якщо використовуєш PlaylistService вже вище — закоментовувати повторно не треба

// ===========================
// ⚙️ Фонова генерація плейлістів + SignalR
// ===========================
builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>(); // Task queue
builder.Services.AddHostedService<PlaylistWorker>(); // Worker that handles queue
builder.Services.AddSignalR(); // SignalR support

// ===========================
// 🚀 App Build
// ===========================
var app = builder.Build();

// ===========================
// 🌐 Middleware Configuration
// ===========================
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); // Додай, якщо використовуєш Identity
app.UseAuthorization();

// ===========================
// 📡 SignalR Hub Routing
// ===========================
app.MapHub<ProgressHub>("/progressHub");

// ===========================
// 📦 MVC Routing
// ===========================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
