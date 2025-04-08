using Nikolo.Data.DTOs.User;
using Nikolo.Data.Models;

namespace Nikolo.Logic.Contracts;

/// <summary>
/// Defines the contract for managing user availability times.
/// </summary>
public interface IUserTimeService
{
    /// <summary>
    /// Adds a new available time slot for an employee.
    /// If a time slot with the same date and start time already exists, it edits the existing slot.
    /// </summary>
    /// <param name="user">The employee for whom the time is being added.</param>
    /// <param name="availableTimeCreateDto">The details of the available time slot.</param>
    /// <returns>The created or updated available time slot, or null if the time range is invalid.</returns>
    Task<AvailableTime?> AddTime(Employee user, AvailableTimeCreateDto availableTimeCreateDto);
    
    /// <summary>
    /// Edits an existing available time slot for an employee.
    /// </summary>
    /// <param name="user">The employee whose time slot is being edited.</param>
    /// <param name="availableTimeEdit">The updated details of the available time slot.</param>
    /// <returns>The updated available time slot, or null if the time slot does not exist.</returns>
    Task<AvailableTime?> EditTime(Employee user, AvailableTimeEditDto availableTimeEdit);
    
    /// <summary>
    /// Retrieves all available time slots for a given employee.
    /// </summary>
    /// <param name="user">The employee whose available time slots are being retrieved.</param>
    /// <returns>A list of available time slots.</returns>
    Task<List<AvailableTime>> GetAvailableTimes(Employee user);
    
    /// <summary>
    /// Retrieves available time slots for a given employee on a specific date.
    /// </summary>
    /// <param name="user">The employee whose available time slots are being retrieved.</param>
    /// <param name="date">The specific date for which available time slots are being retrieved.</param>
    /// <returns>A list of available time slots on the specified date.</returns>
    Task<List<AvailableTime>> GetAvailableTimesForDay(Employee user, DateOnly date);
}