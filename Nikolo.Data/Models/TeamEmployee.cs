namespace Nikolo.Data.Models;

public class TeamEmployee
{
    public int TeamId { get; set; }
    public int EmployeeId { get; set; }
    public required Team Team { get; set; }
    public required Employee Employee { get; set; }
}