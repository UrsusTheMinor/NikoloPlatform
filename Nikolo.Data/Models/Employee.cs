namespace Nikolo.Data.Models;

public class Employee
{
    public int Id { get; set; }
    public string Auth0Id { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}