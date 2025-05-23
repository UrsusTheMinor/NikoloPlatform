using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nikolo.Data;
using Nikolo.Data.DTOs.Skill;
using Nikolo.Data.Models;
using Nikolo.Logic.Contracts;

namespace Nikolo.Logic.Services;

public class SkillService(ApplicationDbContext context, ILogger<UserService> logger) : ISkillService
{
    private readonly ILogger<UserService> logger = logger;

    public async Task CreateSkill(CreateSkillDto skillDto)
    {
        await context.Skills.AddAsync(new Skill()
        {
            SkillName = skillDto.SkillName,
        });
        await context.SaveChangesAsync();
        logger.LogInformation("Successfully added Skill: {Name}", skillDto.SkillName);
    }

    public async Task<List<Skill>> GetAllSkills()
    {
        var skills = await context.Skills.ToListAsync();
        return skills;
    }

    public async Task<Skill?> GetSkill(int skillId)
    {
        return await context.Skills.FirstOrDefaultAsync(x => x.Id == skillId);
    }

    public async Task<bool> DeleteSkill(int skillId)
    {
        var skill = await context.Skills.FindAsync(skillId);
        if (skill == null)
        {
            logger.LogInformation("Skill {SkillId} not found", skillId);
            return false;
        }

        context.Skills.Remove(skill);
        await context.SaveChangesAsync();
        logger.LogInformation("Successfully deleted Skill: {Name}", skill.SkillName);
        return true;
    }

    public async Task<bool> UpdateSkill(int skillId, string skillname)
    {
        var skill = await context.Skills.FindAsync(skillId);
        if (skill == null)
        {
            logger.LogInformation("Skill {SkillId} not found", skillId);
            return false;
        }
        
        skill.SkillName = skillname;
        await context.SaveChangesAsync();
        logger.LogInformation("Successfully updated skill: {Name}", skill.SkillName);
        return true;
    }
    
}