namespace HotelReservation.Services;

using HotelReservation.Infrastructure;
using HotelReservation.Models;

public class ReservationService
{
    private readonly ReservationRepository _repository;
    private readonly ReservationDomainService _domainService;
    private readonly ILogger _logger;
    private static int _counter;

    public ReservationService()
        : this(new ReservationRepository(), new FileLogger())
    {
    }

    public ReservationService(ReservationRepository repository, ILogger logger)
    {
        _repository = repository;
        _domainService = new ReservationDomainService(repository);
        _logger = logger;
    }

    public string CreateReservation(string guestName, string roomId, DateTime checkIn,
        DateTime checkOut, int guestCount, string roomType, string email)
    {
        _logger.Log($"Creating reservation for {guestName}...");

        var room = _domainService.RequireRoom(roomId);
        _domainService.EnsureCapacity(room, guestCount);
        _domainService.EnsureAvailability(room, checkIn, checkOut);
        var total = _domainService.CalculatePrice(room, checkIn, checkOut);

        var reservation = new Reservation
        {
            Id = $"R-{++_counter:D3}",
            GuestName = guestName,
            RoomId = roomId,
            CheckIn = checkIn,
            CheckOut = checkOut,
            GuestCount = guestCount,
            RoomType = roomType,
            Status = "Confirmed",
            Email = email,
            TotalPrice = total
        };

        _repository.Save(reservation);
        _logger.Log($"Reservation {reservation.Id} created.");

        return reservation.Id;
    }

    public Reservation? GetReservation(string id)
    {
        return _repository.GetById(id);
    }

    public List<Reservation> GetAllReservations()
    {
        return _repository.GetAll();
    }

    public List<Room> GetRooms() => _repository.GetRooms();

    public void Reset()
    {
        _repository.Clear();
        _counter = 0;
    }
}
