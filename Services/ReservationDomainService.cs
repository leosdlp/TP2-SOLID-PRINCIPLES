namespace HotelReservation.Services;

using HotelReservation.Models;

public class ReservationDomainService
{
    private readonly ReservationRepository _repository;

    public ReservationDomainService(ReservationRepository repository)
    {
        _repository = repository;
    }

    public Room RequireRoom(string roomId)
    {
        var room = _repository.FindRoom(roomId);
        if (room == null)
            throw new Exception($"Room {roomId} not found");
        return room;
    }

    public void EnsureCapacity(Room room, int guestCount)
    {
        if (guestCount > room.MaxGuests)
            throw new Exception($"Room {room.Id} max capacity is {room.MaxGuests}");
    }

    public void EnsureAvailability(Room room, DateTime checkIn, DateTime checkOut)
    {
        var hasConflict = _repository.GetReservationsForRoom(room.Id)
            .Any(r => r.Status != "Cancelled" && r.CheckIn < checkOut && r.CheckOut > checkIn);
        if (hasConflict)
            throw new Exception($"Room {room.Id} is not available for {checkIn:dd/MM} -> {checkOut:dd/MM}");
    }

    public decimal CalculatePrice(Room room, DateTime checkIn, DateTime checkOut)
    {
        var nights = (checkOut - checkIn).Days;
        return nights * room.PricePerNight;
    }
}
