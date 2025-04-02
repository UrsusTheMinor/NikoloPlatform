using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nikolo.Data;
using Nikolo.Data.Models;
using Nikolo.Logic.Contracts;

namespace Nikolo.Logic.Services;

public class SkillService(ApplicationDbContext context, ILogger<UserService> logger) : ISkillService
{
    private readonly ILogger<UserService> logger = logger;

    public async Task AddSkill(string skillname)
    {
        await context.Skills.AddAsync(new Skill()
        {
            SkillName = skillname,
        });
        await context.SaveChangesAsync();
        logger.LogInformation("Successfully added Skill: {Name}", skillname);
    }

    public async Task<List<Skill>> GetSkills()
    {
        return await context.Skills.ToListAsync();
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