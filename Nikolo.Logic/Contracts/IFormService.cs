using Nikolo.Data.DTOs.InformationForm;
using Nikolo.Data.Models;

namespace Nikolo.Logic.Contracts;

public interface IFormService
{
    Task<InformationGroup?> GetGroupById(int groupId);
    Task<InformationType> SaveInformationType(InformationTypeCreateDto createDto);
    
    Task<InformationGroup> SaveInformationGroup(InformationGroupCreateDto createDto);
}