using Nikolo.Data.DTOs.Skill;
using Nikolo.Data.Models;

namespace Nikolo.Logic.Contracts;

/// <summary>
/// Defines the contract for user management, including user creation, retrieval, and buddy connections.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Creates a new user in the Employees table if they do not already exist.
    /// </summary>
    /// <param name="auth0Id">The unique Auth0 identifier for the user.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<Employee> CreateUser(string auth0Id);
    
    /// <summary>
    /// Retrieves a user based on their Auth0 ID.
    /// </summary>
    /// <param name="auth0Id">The unique Auth0 identifier of the user.</param>
    /// <returns>A task that resolves to an <see cref="Employee"/> instance if found; otherwise, null.</returns>
    Task<Employee?> GetUser(string auth0Id);
    
    /// <summary>
    /// Establishes a buddy connection between two users.
    /// </summary>
    /// <param name="user">The primary user who is adding a buddy.</param>
    /// <param name="buddy">The user to be added as a buddy.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddBuddy(Employee user, Employee buddy);
    
    /// <summary>
    /// Assigns a skill to an employee.
    /// </summary>
    /// <param name="user">The employee to whom the skill will be assigned.</param>
    /// <param name="skillId">The unique identifier of the skill to assign.</param>
    /// <returns>A task that resolves to <c>true</c> if the skill was successfully assigned; otherwise, <c>false</c>.</returns>
    Task<bool> AddSkillToUser(Employee user, int skillId);
    
    Task<List<Skill>> GetSkillsFromUser(Employee user);
    
    Task RemoveSkillFromUser(Employee user, int skillId);
}