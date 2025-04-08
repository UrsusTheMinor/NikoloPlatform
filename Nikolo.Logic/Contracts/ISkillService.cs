using Nikolo.Data.DTOs.Skill;
using Nikolo.Data.Models;

namespace Nikolo.Logic.Contracts;

/// <summary>
/// Defines the contract for managing skills in the application.
/// Provides methods to add, retrieve, update, and delete skills.
/// </summary>
public interface ISkillService
{
    /// <summary>
    /// Adds a new skill to the database.
    /// </summary>
    /// <param name="skillDto">SkillDto which includes information on how a Skill should be created.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateSkill(CreateSkillDto skillDto);
    
    /// <summary>
    /// Retrieves a list of all skills from the database.
    /// </summary>
    /// <returns>A task that resolves to a list of <see cref="Skill"/> objects.</returns>
    Task<List<Skill>> GetAllSkills();

    /// <summary>
    /// Retrieves a skill by its unique identifier.
    /// </summary>
    /// <param name="skillId">The unique identifier of the skill to retrieve.</param>
    /// <returns>A task that resolves to the <see cref="Skill"/> object if found; otherwise, <c>null</c>.</returns>
    Task<Skill?> GetSkill(int skillId);
    
    /// <summary>
    /// Deletes a skill from the database by its ID.
    /// </summary>
    /// <param name="skillId">The unique identifier of the skill to be deleted.</param>
    /// <returns>A task that resolves to <c>true</c> if the deletion was successful; otherwise, <c>false</c>.</returns>
    Task<bool> DeleteSkill(int skillId);
    
    /// <summary>
    /// Updates the name of an existing skill.
    /// </summary>
    /// <param name="skillId">The unique identifier of the skill to be updated.</param>
    /// <param name="skillname">The new name of the skill.</param>
    /// <returns>A task that resolves to <c>true</c> if the update was successful; otherwise, <c>false</c>.</returns>
    Task<bool> UpdateSkill(int skillId, string skillname);
}