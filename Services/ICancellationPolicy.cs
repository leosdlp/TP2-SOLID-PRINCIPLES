namespace HotelReservation.Services;

using HotelReservation.Models;

public interface ICancellationPolicy
{
    string Name { get; }
    decimal CalculateRefund(Reservation reservation, DateTime requestDate);
}
