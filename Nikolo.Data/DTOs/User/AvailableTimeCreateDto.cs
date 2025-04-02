namespace Nikolo.Data.DTOs.User;

public class AvailableTimeCreateDto
{
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}