namespace HotelReservation.Repositories;

using HotelReservation.Models;
using HotelReservation.Services;

public class InMemoryReservationRepository : IReservationRepository
{
    private readonly Dictionary<string, Reservation> _reservations = new();
    private readonly BillingCalculator _billingCalculator = new();

    public Reservation? GetById(string id)
    {
        return _reservations.TryGetValue(id, out var r) ? r : null;
    }

    public List<Reservation> GetAll()
    {
        return _reservations.Values.ToList();
    }

    public List<Reservation> GetByDateRange(DateTime from, DateTime to)
    {
        return _reservations.Values
            .Where(r => r.CheckIn < to && r.CheckOut > from && r.Status != "Cancelled")
            .ToList();
    }

    public List<Reservation> GetByGuest(string guestName)
    {
        return _reservations.Values
            .Where(r => r.GuestName == guestName)
            .ToList();
    }

    public void Add(Reservation reservation)
    {
        _reservations[reservation.Id] = reservation;
    }

    public void Update(Reservation reservation)
    {
        _reservations[reservation.Id] = reservation;
    }

    public void Delete(string id)
    {
        _reservations.Remove(id);
    }

    public decimal GetTotalRevenue(DateTime from, DateTime to)
    {
        return _reservations.Values
            .Where(r => r.CheckIn >= from && r.CheckOut <= to && r.Status != "Cancelled")
            .Sum(r => _billingCalculator.Calculate(r).Total);
    }

    public Dictionary<string, int> GetOccupancyStats(DateTime from, DateTime to)
    {
        var reservations = GetByDateRange(from, to);
        return reservations
            .GroupBy(r => r.RoomType)
            .ToDictionary(g => g.Key, g => g.Count());
    }
}
