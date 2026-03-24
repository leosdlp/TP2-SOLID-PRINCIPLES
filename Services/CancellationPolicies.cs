namespace HotelReservation.Services;

using HotelReservation.Models;

public class FlexiblePolicy : ICancellationPolicy
{
    public string Name => "Flexible";

    public decimal CalculateRefund(Reservation reservation, DateTime requestDate)
    {
        var daysBefore = (reservation.CheckIn - requestDate).Days;
        return daysBefore >= 1 ? reservation.TotalPrice : 0m;
    }
}

public class ModeratePolicy : ICancellationPolicy
{
    public string Name => "Moderate";

    public decimal CalculateRefund(Reservation reservation, DateTime requestDate)
    {
        var daysBefore = (reservation.CheckIn - requestDate).Days;
        if (daysBefore >= 5) return reservation.TotalPrice;
        if (daysBefore >= 2) return reservation.TotalPrice * 0.5m;
        return 0m;
    }
}

public class StrictPolicy : ICancellationPolicy
{
    public string Name => "Strict";

    public decimal CalculateRefund(Reservation reservation, DateTime requestDate)
    {
        var daysBefore = (reservation.CheckIn - requestDate).Days;
        if (daysBefore >= 14) return reservation.TotalPrice;
        if (daysBefore >= 7) return reservation.TotalPrice * 0.5m;
        return 0m;
    }
}

public class NonRefundablePolicy : ICancellationPolicy
{
    public string Name => "NonRefundable";

    public decimal CalculateRefund(Reservation reservation, DateTime requestDate)
    {
        return 0m;
    }
}
