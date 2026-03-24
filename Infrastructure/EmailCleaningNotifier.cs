namespace HotelReservation.Infrastructure;

using HotelReservation.Models;
using HotelReservation.Services;

public class EmailCleaningNotifier : ICleaningNotifier
{
    private readonly EmailSender _emailSender;

    public EmailCleaningNotifier(EmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public void Notify(CleaningTask task)
    {
        _emailSender.Send(
            task.HousekeeperEmail,
            "New cleaning task",
            $"Room {task.RoomId} needs {task.Type} on {task.Date:dd/MM/yyyy}");
    }
}
