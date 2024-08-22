using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FTSept2022.Aufgabe1.Test
{
    public class Aufgabe1Test
    {
        [Fact]
        public void CreateDatabaseTest()
        {
            var options = new DbContextOptionsBuilder()
                .UseSqlite(@"Data Source=Scooter.db")
                .Options;

            var db = new ScooterContext(options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            Assert.True(true);
        }
    }
}