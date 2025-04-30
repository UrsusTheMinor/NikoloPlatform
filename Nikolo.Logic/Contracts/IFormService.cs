using Nikolo.Data.DTOs.InformationForm;
using Nikolo.Data.DTOs.InformationForm.Group;
using Nikolo.Data.DTOs.InformationForm.Type;
using Nikolo.Data.Models;
using Nikolo.Data.Models.Form;

namespace Nikolo.Logic.Contracts;

public interface IFormService
{
    Task<InformationGroup?> GetGroupById(int? groupId);
    Task<InformationType> SaveInformationType(InformationTypeCreateDto createDto);
    Task<InformationType?> EditInformationType(InformationTypeEditDto editDto);
    Task DeleteInformationType(int id);
    Task<List<InformationTypeReturnDto>> GetAllInformationTypes();
    Task<InformationTypeReturnDto?> GetInformationTypeById(int id);
    
    // TODO: InformationGroup: Edit, Delete
    Task<InformationGroup> SaveInformationGroup(InformationGroupCreateDto createDto);
    Task<InformationGroup?> EditInformationGroup(InformationGroupEditDto editDto);
    Task DeleteInformationGroup(int id);
    Task<List<InformationGroupReturnDto>> GetAllInformationGroups();
    Task<InformationGroupReturnDto?> GetInformationGroupById(int id);
    
    
    Task<bool> Move(MoveDto moveDto);
}