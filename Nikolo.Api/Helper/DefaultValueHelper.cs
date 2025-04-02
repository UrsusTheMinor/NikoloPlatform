using Nikolo.Logic.Contracts;

namespace Nikolo.Api.Helper;

public static class DefaultValueHelper
{
    public static async Task SeedDefaultSkills(ISkillService skillService, List<string> defaultSkills)
    {
        var existingSkills = await skillService.GetSkills();

        foreach (var skill in defaultSkills)
        {
            if (!existingSkills.Any(s => s.SkillName == skill))
            {
                await skillService.AddSkill(skill);
            }
        }
    }
}