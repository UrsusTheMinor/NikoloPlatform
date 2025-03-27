namespace Nikolo.Data.Models;

public class TeamBuddy
{
    public int Employee1Id { get; set; }
    public int Employee2Id { get; set; }
    
    public required Employee Employee1 { get; set; }
    public required Employee Employee2 { get; set; }
}