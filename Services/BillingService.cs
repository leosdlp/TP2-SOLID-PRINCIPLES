namespace HotelReservation.Services;

using HotelReservation.Repositories;

public class BillingService
{
    private readonly IReservationRevenueProvider _repo;

    public BillingService(IReservationRevenueProvider repo)
    {
        _repo = repo;
    }

    public decimal GetRevenueForPeriod(DateTime from, DateTime to)
    {
        return _repo.GetTotalRevenue(from, to);
    }
}
