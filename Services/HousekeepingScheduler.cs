namespace HotelReservation.Services;

using HotelReservation.Models;

public class HousekeepingScheduler
{
    public List<DateTime> GetLinenChangeDays(Reservation reservation, int frequencyDays = 3)
    {
        var result = new List<DateTime>();
        var current = reservation.CheckIn.AddDays(frequencyDays);
        while (current < reservation.CheckOut)
        {
            result.Add(current);
            current = current.AddDays(frequencyDays);
        }
        return result;
    }
}
