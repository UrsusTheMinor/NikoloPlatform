using Nikolo.Data.DTOs.User;

namespace Nikolo.Api.Responses.User;

/// <summary>
/// Response model for a successfully created user.
/// </summary>
public class UserCreatedResponse
{
    public string Message { get; set; } = "User saved successfully";
    public UserDto User { get; set; } = default!;
}