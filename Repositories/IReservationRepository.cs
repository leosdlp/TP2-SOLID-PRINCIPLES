namespace HotelReservation.Repositories;

using HotelReservation.Models;

public interface IReservationReader
{
    Reservation? GetById(string id);
    List<Reservation> GetAll();
    List<Reservation> GetByDateRange(DateTime from, DateTime to);
    List<Reservation> GetByGuest(string guestName);
}

public interface IReservationWriter
{
    void Add(Reservation reservation);
    void Update(Reservation reservation);
    void Delete(string id);
}

public interface IReservationRevenueProvider
{
    decimal GetTotalRevenue(DateTime from, DateTime to);
}

public interface IReservationOccupancyProvider
{
    Dictionary<string, int> GetOccupancyStats(DateTime from, DateTime to);
}

public interface IReservationRepository :
    IReservationReader,
    IReservationWriter,
    IReservationRevenueProvider,
    IReservationOccupancyProvider
{
}
