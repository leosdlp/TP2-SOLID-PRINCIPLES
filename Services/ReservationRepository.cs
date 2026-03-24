namespace HotelReservation.Services;

using HotelReservation.Models;

public class ReservationRepository
{
    private readonly Dictionary<string, Reservation> _reservations = new();
    private readonly List<Room> _rooms = new()
    {
        new Room { Id = "101", Type = "Standard", MaxGuests = 2, PricePerNight = 80m },
        new Room { Id = "102", Type = "Standard", MaxGuests = 2, PricePerNight = 80m },
        new Room { Id = "201", Type = "Suite", MaxGuests = 2, PricePerNight = 200m },
        new Room { Id = "301", Type = "Family", MaxGuests = 4, PricePerNight = 120m }
    };

    public Room? FindRoom(string roomId) => _rooms.FirstOrDefault(r => r.Id == roomId);

    public List<Room> GetRooms()
    {
        return _rooms.Select(r => new Room
        {
            Id = r.Id,
            Type = r.Type,
            MaxGuests = r.MaxGuests,
            PricePerNight = r.PricePerNight
        }).ToList();
    }

    public List<Reservation> GetReservationsForRoom(string roomId)
    {
        return _reservations.Values
            .Where(r => r.RoomId == roomId)
            .ToList();
    }

    public void Save(Reservation reservation)
    {
        _reservations[reservation.Id] = reservation;
    }

    public Reservation? GetById(string id)
    {
        return _reservations.TryGetValue(id, out var reservation) ? reservation : null;
    }

    public List<Reservation> GetAll()
    {
        return _reservations.Values.ToList();
    }

    public void Clear()
    {
        _reservations.Clear();
    }
}
