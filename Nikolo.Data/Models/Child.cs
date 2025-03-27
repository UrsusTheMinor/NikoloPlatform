namespace Nikolo.Data.Models;

public class Child
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string LobUndTadel { get; set; } = string.Empty;
    public Booking? Booking { get; set; }
}