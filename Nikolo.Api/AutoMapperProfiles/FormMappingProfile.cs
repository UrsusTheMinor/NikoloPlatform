using AutoMapper;
using Nikolo.Data.DTOs.InformationForm;
using Nikolo.Data.Models;

namespace Nikolo.Api.AutoMapperProfiles;

public class FormMappingProfile : Profile
{
    public FormMappingProfile()
    {
        CreateMap<InformationTypeCreateDto, InformationType>()
            .ForMember(dest => dest.Group, opt => opt.Ignore());
        CreateMap<InformationGroupCreateDto, InformationGroup>();
    }
}