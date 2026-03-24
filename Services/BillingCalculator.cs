namespace HotelReservation.Services;

using HotelReservation.Models;

public record BillingBreakdown(int Nights, decimal Subtotal, decimal Tva, decimal TouristTax, decimal Total);

public class BillingCalculator
{
    public BillingBreakdown Calculate(Reservation reservation)
    {
        var nights = (reservation.CheckOut - reservation.CheckIn).Days;
        var pricePerNight = reservation.RoomType switch
        {
            "Standard" => 80m,
            "Suite" => 200m,
            "Family" => 120m,
            _ => 0m
        };

        var subtotal = nights * pricePerNight;
        var tva = subtotal * 0.10m;
        var touristTax = reservation.GuestCount * nights * 1.50m;
        var total = subtotal + tva + touristTax;

        return new BillingBreakdown(nights, subtotal, tva, touristTax, total);
    }

    public string GenerateInvoiceLine(Reservation reservation)
    {
        var breakdown = Calculate(reservation);
        return $"{reservation.GuestName} | {reservation.CheckIn:dd/MM} -> {reservation.CheckOut:dd/MM} | {breakdown.Total:F2} EUR";
    }
}
