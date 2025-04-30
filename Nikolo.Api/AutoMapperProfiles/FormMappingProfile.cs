using AutoMapper;
using Nikolo.Data.DTOs.InformationForm;
using Nikolo.Data.DTOs.InformationForm.Group;
using Nikolo.Data.DTOs.InformationForm.Type;
using Nikolo.Data.Models;
using Nikolo.Data.Models.Form;

namespace Nikolo.Api.AutoMapperProfiles;

public class FormMappingProfile : Profile
{
    public FormMappingProfile()
    {
        CreateMap<InformationTypeCreateDto, InformationType>()
            .ForMember(dest => dest.Group, opt => opt.Ignore());
        CreateMap<InformationGroupCreateDto, InformationGroup>();
        CreateMap<InformationType, InformationTypeReturnDto>();
    }
}