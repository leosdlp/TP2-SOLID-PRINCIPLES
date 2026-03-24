namespace HotelReservation.Services;

public class InvoiceRequest
{
    public string ReservationId { get; init; } = string.Empty;
    public string GuestName { get; init; } = string.Empty;
    public string RoomId { get; init; } = string.Empty;
    public string RoomType { get; init; } = string.Empty;
    public DateTime CheckIn { get; init; }
    public DateTime CheckOut { get; init; }
    public int GuestCount { get; init; }
}
