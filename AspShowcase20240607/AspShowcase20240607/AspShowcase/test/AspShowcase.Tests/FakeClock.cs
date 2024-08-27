using AspShowcase.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspShowcase.Tests
{
    public class FakeClock : IClockService
    {
        private readonly DateTime _now;

        public FakeClock(DateTime now)
        {
            _now = now;
        }

        public DateTime Now => _now.Date;
        public DateTime UtcNow => _now.Date;
    }
}
