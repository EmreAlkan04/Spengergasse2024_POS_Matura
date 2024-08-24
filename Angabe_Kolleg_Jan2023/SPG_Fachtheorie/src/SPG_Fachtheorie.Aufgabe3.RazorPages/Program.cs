using SPG_Fachtheorie.Aufgabe2.Infrastructure;
using SPG_Fachtheorie.Aufgabe3.RazorPages.Classes;
using Microsoft.EntityFrameworkCore;

var options = new DbContextOptionsBuilder()
    .UseSqlite("Data Source=Podcast.db")
    .Options;

using (var db = new PodcastContext(options))
{
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
    db.Seed();
}

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages(); 
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<PodcastContext>(opt => opt.UseSqlite("Data Source=Podcast.db"));
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AuthService>();
builder.Services.AddAuthentication(
    Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/";
        o.AccessDeniedPath = "/NotAuthorized";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
