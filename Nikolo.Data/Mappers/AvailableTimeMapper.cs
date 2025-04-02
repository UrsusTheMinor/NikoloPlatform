using Nikolo.Data.DTOs.User;
using Nikolo.Data.Models;

namespace Nikolo.Data.Mappers;

public static class AvailableTimeMapper
{
    public static AvailableTimeEditDto ToAvailableTimeDto(this AvailableTime availableTime)
    {
        return new AvailableTimeEditDto
        {
            Id = availableTime.Id,
            Date = availableTime.Date,
            StartTime = availableTime.StartTime,
            EndTime = availableTime.EndTime,
        };
    }
    
    public static AvailableTimeReturnDto ToAvailableTimeReturnDto(this AvailableTime availableTime)
    {
        return new AvailableTimeReturnDto
        {
            Id = availableTime.Id,
            Date = availableTime.Date,
            StartTime = availableTime.StartTime,
            EndTime = availableTime.EndTime,
        };
    }


    public static AvailableTimeEditDto ToAvailableTimeEditDto(this AvailableTimeCreateDto availableTime, int id)
    {
        return new AvailableTimeEditDto()
        {
            Id = id,
            Date = availableTime.Date,
            StartTime = availableTime.StartTime,
            EndTime = availableTime.EndTime
        };
    }
}