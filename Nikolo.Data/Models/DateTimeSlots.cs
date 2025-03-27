namespace Nikolo.Data.Models;

public class DateTimeSlots
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int AvailableSlots { get; set; }
}