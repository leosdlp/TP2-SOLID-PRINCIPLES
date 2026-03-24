namespace HotelReservation.Services;

using HotelReservation.Models;

public class CheckInService
{
    private readonly ReservationStatusCache _statusCache = new();
    private readonly LateCheckInFeePolicy _lateFeePolicy = new(25m);
    private readonly RoomOccupancyNotifier _notifier = new();

    public CheckInService(Dictionary<string, Reservation> dataStore)
    {
        // dataStore kept for backwards compatibility in Program but no longer directly used here
    }

    public void ProcessCheckIn(Reservation reservation)
    {
        EnsureConfirmed(reservation);
        _statusCache.Update(reservation.Id, "CheckedIn");
        _lateFeePolicy.Apply(reservation, DateTime.Now);
        reservation.Status = "CheckedIn";
        _notifier.NotifyOccupied(reservation.RoomId);
    }

    public void ProcessCheckOut(Reservation reservation)
    {
        EnsureCheckedIn(reservation);
        reservation.Status = "CheckedOut";
        _statusCache.Remove(reservation.Id);
        _notifier.NotifyVacated(reservation.RoomId);
    }

    private static void EnsureConfirmed(Reservation reservation)
    {
        if (reservation.Status != "Confirmed")
            throw new Exception($"Cannot check in: reservation is {reservation.Status}");
    }

    private static void EnsureCheckedIn(Reservation reservation)
    {
        if (reservation.Status != "CheckedIn")
            throw new Exception($"Cannot check out: reservation is {reservation.Status}");
    }
}

internal class ReservationStatusCache
{
    private readonly Dictionary<string, CacheEntry> _cache = new();

    public void Update(string reservationId, string status)
    {
        _cache[reservationId] = new CacheEntry(DateTime.Now, status);
    }

    public void Remove(string reservationId)
    {
        if (_cache.ContainsKey(reservationId))
            _cache.Remove(reservationId);
    }
}

internal class LateCheckInFeePolicy
{
    private readonly decimal _lateFee;

    public LateCheckInFeePolicy(decimal lateFee)
    {
        _lateFee = lateFee;
    }

    public void Apply(Reservation reservation, DateTime now)
    {
        if (now.Hour >= 22)
            reservation.TotalPrice += _lateFee;
    }
}

internal class RoomOccupancyNotifier
{
    public void NotifyOccupied(string roomId)
    {
        Console.WriteLine($"[SMS] Room {roomId} is now occupied");
    }

    public void NotifyVacated(string roomId)
    {
        Console.WriteLine($"[SMS] Room {roomId} is now free");
    }
}
