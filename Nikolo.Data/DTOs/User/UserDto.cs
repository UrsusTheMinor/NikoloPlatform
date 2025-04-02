namespace Nikolo.Data.DTOs.User;

/// <summary>
/// User data transfer object.
/// </summary>
public class UserDto
{
    public int Id { get; set; }
    public string Auth0Id { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}