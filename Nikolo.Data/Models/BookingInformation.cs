namespace Nikolo.Data.Models;

public class BookingInformation
{
    public int Id { get; set; }
    public required Booking Booking { get; set; }
    public required InformationType Type { get; set; }
    public string Value { get; set; } = string.Empty;
}