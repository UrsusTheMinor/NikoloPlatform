using Nikolo.Data.DTOs.Skill;
using Nikolo.Logic.Contracts;

namespace Nikolo.Api.Helper;

public static class DefaultValueHelper
{
    public static async Task SeedDefaultSkills(ISkillService skillService, List<string> defaultSkills)
    {
        var existingSkills = await skillService.GetAllSkills();

        foreach (var skill in defaultSkills)
        {
            if (!existingSkills.Any(s => s.SkillName == skill))
            {
                await skillService.CreateSkill(new CreateSkillDto()
                {
                    SkillName = skill,
                });
            }
        }
    }
}