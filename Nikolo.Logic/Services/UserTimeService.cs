using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nikolo.Data;
using Nikolo.Data.DTOs.User;
using Nikolo.Data.Models;
using Nikolo.Logic.Contracts;

namespace Nikolo.Logic.Services;

public class UserTimeService(ApplicationDbContext context, ILogger<UserTimeService> logger) : IUserTimeService
{
    private readonly ILogger<UserTimeService> logger = logger;

    public async Task<AvailableTime?> AddTime(Employee user, AvailableTimeCreateDto availableTimeCreateDto)
    {
        var overlappingTimes = await context.AvailableTimes
            .Where(x => x.Date == availableTimeCreateDto.Date &&
                        x.StartTime < availableTimeCreateDto.EndTime &&
                        x.EndTime > availableTimeCreateDto.StartTime)
            .ToListAsync();

        if (overlappingTimes.Count > 1)
        {
            return null; // Exit early if more than one overlapping time is found
        }

        var time = overlappingTimes.FirstOrDefault();



        if (time != null)
        {
            logger.LogInformation("Found AvailableTime with same date and start time, exiting.");
            return null;
        }
        
        if (availableTimeCreateDto.EndTime <= availableTimeCreateDto.StartTime)
        {
            logger.LogWarning("Invalid time range: EndTime must be after StartTime.");
            return null;
        }


        var newTime = new AvailableTime()
        {
            Date = availableTimeCreateDto.Date,
            StartTime = availableTimeCreateDto.StartTime,
            EndTime = availableTimeCreateDto.EndTime,
            Employee = user
        };
        
        await context.AvailableTimes.AddAsync(newTime);
        await context.SaveChangesAsync();
        return newTime;
    }

    public async Task<AvailableTime?> EditTime(Employee user, AvailableTimeEditDto availableTimeEdit)
    {
        var time = await context.AvailableTimes.FirstOrDefaultAsync(x => x.Id == availableTimeEdit.Id);

        if (time == null)
        {
            return null;
        }

        // Determine the new start and end times (if provided)
        var newStartTime = availableTimeEdit.StartTime ?? time.StartTime;
        var newEndTime = availableTimeEdit.EndTime ?? time.EndTime;

        // Check if the new times overlap with any other existing times (excluding itself)
        var overlappingTimes = await context.AvailableTimes
            .Where(x => x.Id != time.Id && // Exclude the current time being edited
                        x.Date == time.Date &&
                        x.StartTime < newEndTime &&
                        x.EndTime > newStartTime)
            .ToListAsync();

        if (overlappingTimes.Any()) 
        {
            return null; // Overlap detected, return early
        }

        // Update properties only if they have values
        if (availableTimeEdit.StartTime.HasValue)
        {
            time.StartTime = availableTimeEdit.StartTime.Value;
        }

        if (availableTimeEdit.EndTime.HasValue)
        {
            time.EndTime = availableTimeEdit.EndTime.Value;
        }

        await context.SaveChangesAsync();
        return time;
    }



    public async Task<List<AvailableTime>> GetAvailableTimes(Employee user)
    {
        return await context.AvailableTimes
            .Where(x => x.Employee == user)
            .ToListAsync();
    }

    public async Task<List<AvailableTime>> GetAvailableTimesForDay(Employee user, DateOnly date)
    {
        return await context.AvailableTimes
            .Where(x => x.Employee == user)
            .Where(x => x.Date == date)
            .ToListAsync();
    }
}