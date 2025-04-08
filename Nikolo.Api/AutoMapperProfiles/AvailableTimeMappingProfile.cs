using AutoMapper;
using Nikolo.Data.DTOs.Skill;
using Nikolo.Data.DTOs.User;
using Nikolo.Data.Models;

namespace Nikolo.Api.AutoMapperProfiles;

public class AvailableTimeMappingProfile : Profile
{
    public AvailableTimeMappingProfile()
    {
        CreateMap<AvailableTime, AvailableTimeEditDto>();
        CreateMap<AvailableTime, AvailableTimeReturnDto>();
        CreateMap<AvailableTimeCreateDto, AvailableTimeEditDto>();
    }
}