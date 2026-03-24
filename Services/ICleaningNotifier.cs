namespace HotelReservation.Services;

using HotelReservation.Models;

public interface ICleaningNotifier
{
    void Notify(CleaningTask task);
}
