using AutoMapper;
using Nikolo.Data.DTOs.Skill;
using Nikolo.Data.Models;

namespace Nikolo.Api.AutoMapperProfiles;

public class SkillMappingProfile : Profile
{
    public SkillMappingProfile()
    {
        CreateMap<Skill, SkillDto>();
    }
}