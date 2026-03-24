namespace HotelReservation.Services;

using HotelReservation.Models;

public class InvoiceGenerator
{
    private readonly BillingCalculator _calculator = new();

    public Invoice Generate(InvoiceRequest request)
    {
        var breakdown = _calculator.Calculate(new Reservation
        {
            RoomType = request.RoomType,
            CheckIn = request.CheckIn,
            CheckOut = request.CheckOut,
            GuestCount = request.GuestCount
        });

        return new Invoice
        {
            ReservationId = request.ReservationId,
            GuestName = request.GuestName,
            RoomDescription = $"{request.RoomType} {request.RoomId}",
            Nights = breakdown.Nights,
            Subtotal = breakdown.Subtotal,
            Tva = breakdown.Tva,
            TouristTax = breakdown.TouristTax,
            Total = breakdown.Total
        };
    }

    public void PrintInvoice(Invoice invoice, InvoiceRequest request)
    {
        Console.WriteLine($"Invoice for {invoice.GuestName}:");
        Console.WriteLine($"  Room: {invoice.RoomDescription}, " +
            $"{request.CheckIn:dd/MM} -> {request.CheckOut:dd/MM} " +
            $"({invoice.Nights} nights)");
        Console.WriteLine($"  Subtotal: {invoice.Subtotal:F2} EUR");
        Console.WriteLine($"  TVA (10%): {invoice.Tva:F2} EUR");
        Console.WriteLine($"  Tourist Tax ({request.GuestCount} guests x " +
            $"{invoice.Nights} nights x 1.50 EUR): {invoice.TouristTax:F2} EUR");
        Console.WriteLine($"  Total: {invoice.Total:F2} EUR");
    }
}
