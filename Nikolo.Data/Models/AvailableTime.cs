namespace Nikolo.Data.Models;

public class AvailableTime
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public required Employee Employee { get; set; }
}