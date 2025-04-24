using Nikolo.Data.DTOs.InformationForm;
using Nikolo.Data.Models;

namespace Nikolo.Logic.Contracts;

public interface IFormService
{
    Task<InformationGroup?> GetGroupById(int? groupId);
    Task<InformationType> SaveInformationType(InformationTypeCreateDto createDto);
    Task<InformationType?> EditInformationType(InformationTypeEditDto editDto);
    Task DeleteInformationType(int id);
    Task<bool> InformationTypeMove(InformationTypeMoveDto moveDto);
    Task<List<InformationTypeReturnDto>> GetAllInformationTypes();
    Task<InformationTypeReturnDto?> GetInformationTypeById(int id);
    
    // TODO: InformationGroup: Edit, Delete
    Task<InformationGroup> SaveInformationGroup(InformationGroupCreateDto createDto);
}