namespace Nikolo.Data.DTOs;

public class UserUpsertDto
{
    public string Auth0Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
}