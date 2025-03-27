namespace Nikolo.Data.Models;

public class Booking
{
    public int Id { get; set; }
    public string RecoverToken { get; set; } = string.Empty;
    public Team? Team { get; set; }
    public DateTimeSlots? DateTimeSlots { get; set; }
}