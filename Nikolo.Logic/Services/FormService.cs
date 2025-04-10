using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nikolo.Data;
using Nikolo.Data.DTOs.InformationForm;
using Nikolo.Data.Models;
using Nikolo.Logic.Contracts;

namespace Nikolo.Logic.Services;

public class FormService(ApplicationDbContext context, ILogger<UserService> logger, IMapper mapper) : IFormService
{
    private readonly IMapper mapper = mapper;

    public async Task<InformationGroup?> GetGroupById(int groupId)
    {
        return await context.InformationGroups.Where(x => x.Id == groupId).FirstOrDefaultAsync();
    }

    public async Task<InformationType> SaveInformationType(InformationTypeCreateDto createDto)
    {
        var group = await GetGroupById(createDto.GroupId);

        var infoType = mapper.Map<InformationType>(createDto);
        infoType.Group = group;

        context.InformationTypes.Add(infoType);

        var itemsToUpdate = context.InformationTypes
            .Where(i => i.Group == group && i.Index >= createDto.Index);

        foreach (var item in itemsToUpdate)
        {
            item.Index += 1;
        }


        if (group == null)
        {
            var groupsToUpdate = context.InformationGroups
                .Where(i => i.Index >= createDto.Index);

            foreach (var grp in groupsToUpdate)
            {
                grp.Index += 1;
            }
        }

        await context.SaveChangesAsync();
        return infoType;
    }

    public async Task<InformationGroup> SaveInformationGroup(InformationGroupCreateDto createDto)
    {
        var group = mapper.Map<InformationGroup>(createDto);
        context.InformationGroups.Add(group);

        var itemsToUpdate = context.InformationTypes
            .Where(i => i.Index >= createDto.Index);

        foreach (var item in itemsToUpdate)
        {
            item.Index += 1;
        }


        var groupsToUpdate = context.InformationGroups
            .Where(i => i.Index >= createDto.Index);

        foreach (var grp in groupsToUpdate)
        {
            grp.Index += 1;
        }
        
        await context.SaveChangesAsync();
        return group;
    }
}