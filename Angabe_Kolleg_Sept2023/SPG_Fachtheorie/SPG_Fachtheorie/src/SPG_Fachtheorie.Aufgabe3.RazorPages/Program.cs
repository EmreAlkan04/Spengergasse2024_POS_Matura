using SPG_Fachtheorie.Aufgabe2.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var options = new DbContextOptionsBuilder()
    .UseSqlite("Data Source=sticker.db")
    .Options;

using (var db = new StickerContext(options))
{
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
    db.Seed();
}

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddDbContext<StickerContext>(opt => opt.UseSqlite("Data Source=sticker.db"));
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.Run();