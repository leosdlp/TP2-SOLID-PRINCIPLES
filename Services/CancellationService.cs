namespace HotelReservation.Services;

using HotelReservation.Models;

public class CancellationService
{
    private readonly Dictionary<string, ICancellationPolicy> _policies;

    public CancellationService()
        : this(new ICancellationPolicy[]
        {
            new FlexiblePolicy(),
            new ModeratePolicy(),
            new StrictPolicy(),
            new NonRefundablePolicy()
        })
    {
    }

    public CancellationService(IEnumerable<ICancellationPolicy> policies)
    {
        _policies = policies.ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
    }

    public decimal CalculateRefund(Reservation reservation, DateTime now)
    {
        var policy = ResolvePolicy(reservation.CancellationPolicy);
        return policy.CalculateRefund(reservation, now);
    }

    public void CancelReservation(Reservation reservation, DateTime now)
    {
        var refund = CalculateRefund(reservation, now);
        reservation.Cancel();
        Console.WriteLine(
            $"[OK] Reservation {reservation.Id} cancelled " +
            $"({reservation.CancellationPolicy} policy: " +
            $"{(refund == reservation.TotalPrice ? "full" : "partial")} refund of {refund:F2} EUR)");
    }

    private ICancellationPolicy ResolvePolicy(string name)
    {
        if (_policies.TryGetValue(name, out var policy))
            return policy;

        throw new ArgumentException($"Unknown cancellation policy: {name}");
    }
}
