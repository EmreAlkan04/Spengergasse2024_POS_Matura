using System;

namespace AspShowcase.Application.Services
{
    public interface IClockService
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }
}
