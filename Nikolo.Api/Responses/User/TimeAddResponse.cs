using Nikolo.Data.DTOs.User;

namespace Nikolo.Api.Responses.User;

public class TimeAddResponse
{
    public string Message { get; set; } = "User saved successfully";
    public AvailableTimeReturnDto AvailableTime { get; set; } = null!;
    public UserDto User { get; set; } = null!;
}