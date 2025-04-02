using Nikolo.Data.DTOs.Skill;
using Nikolo.Data.Models;

namespace Nikolo.Data.Mappers;

public static class SkillMapper
{

    public static SkillDto ToSkillDto(this Skill skillModel)
    {
        return new SkillDto()
        {
            Id = skillModel.Id,
            SkillName = skillModel.SkillName
        };
    }
}