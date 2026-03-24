namespace HotelReservation.Services;

using HotelReservation.Models;

public interface IReservationRepository
{
    void Add(Reservation reservation);
    Reservation? GetById(string id);
    List<Reservation> GetAll();
}
