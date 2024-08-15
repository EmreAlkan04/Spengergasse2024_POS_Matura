using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe2.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var options = new DbContextOptionsBuilder()
    .UseSqlite("Data Source=sticker.db")
    .Options;

using (var db = new StickerContext(options))
{
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
    db.Seed();
}

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<StickerContext>(opt => opt.UseSqlite("Data Source=sticker.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
