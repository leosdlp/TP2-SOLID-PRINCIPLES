namespace HotelReservation.Infrastructure;

using HotelReservation.Services;

// Simulates file-based logging (uses Console for demo purposes).
// The SOLID violation is the direct coupling, not the I/O mechanism.
public class FileLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine($"[LOG] {message}");
    }
}
