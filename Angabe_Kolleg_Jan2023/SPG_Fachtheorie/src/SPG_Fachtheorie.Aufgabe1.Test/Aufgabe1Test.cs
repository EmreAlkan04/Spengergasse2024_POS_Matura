using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe1.Infrastructure;
using Xunit;

namespace SPG_Fachtheorie.Aufgabe1.Test
{
    public class Aufgabe1Test
    {
        [Fact]
        public void CreateDatabaseTest()
        {
            var options = new DbContextOptionsBuilder()
                .UseSqlite(@"Data Source=User.db")
                .Options;

            var db = new UserContext(options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            Assert.True(true);
        }
    }
}