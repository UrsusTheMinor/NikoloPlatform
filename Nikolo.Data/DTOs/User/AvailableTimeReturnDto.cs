namespace Nikolo.Data.DTOs.User;

public class AvailableTimeReturnDto
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}