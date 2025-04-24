using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nikolo.Data;
using Nikolo.Data.DTOs.InformationForm;
using Nikolo.Data.Extensions;
using Nikolo.Data.Models;
using Nikolo.Logic.Contracts;

namespace Nikolo.Logic.Services;

public class FormService(ApplicationDbContext context, ILogger<UserService> logger, IMapper mapper) : IFormService
{
    private readonly IMapper mapper = mapper;

    public async Task<InformationGroup?> GetGroupById(int? groupId)
    {
        return await context.InformationGroups.Where(x => x.Id == groupId).FirstOrDefaultAsync();
    }

    public async Task<InformationType> SaveInformationType(InformationTypeCreateDto createDto)
    {
        var group = await GetGroupById(createDto.GroupId);

        var infoType = mapper.Map<InformationType>(createDto);
        infoType.Group = group;


        var itemsToUpdate = context.InformationTypes
            .NotDeleted()
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

        context.InformationTypes.Add(infoType);


        await context.SaveChangesAsync();
        return infoType;
    }

    public async Task<InformationType?> EditInformationType(InformationTypeEditDto editDto)
    {
        var infoType = await context.InformationTypes
            .NotDeleted()
            .FirstOrDefaultAsync(x => x.Id == editDto.Id);

        if (infoType == null)
        {
            return null;
        }

        if (!string.IsNullOrWhiteSpace(editDto.TypeName))
        {
            infoType.TypeName = editDto.TypeName;
        }

        if (!string.IsNullOrWhiteSpace(editDto.FieldType))
        {
            infoType.FieldType = editDto.FieldType;
        }

        if (editDto.Width.HasValue)
        {
            infoType.Width = editDto.Width.Value;
        }

        if (!string.IsNullOrWhiteSpace(editDto.Placeholder))
        {
            infoType.Placeholder = editDto.Placeholder;
        }

        infoType.ModifiedOn = DateTime.Now;

        await context.SaveChangesAsync();
        return infoType;
    }

    public async Task DeleteInformationType(int id)
    {
        var infoType = await context.InformationTypes
            .NotDeleted()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (infoType == null)
        {
            return;
        }

        infoType.DeletedOn = DateTime.Now;
        infoType.Group = null;
        infoType.Index = -1;

        var affectedTypes = await context.InformationTypes
            .NotDeleted()
            .Where(x => x.Index > infoType.Index)
            .ToListAsync();

        var affectedGroups = await context.InformationGroups
            .Where(x => x.Index > infoType.Index)
            .ToListAsync();

        foreach (var info in affectedTypes)
        {
            info.Index -= 1;
        }

        foreach (var group in affectedGroups)
        {
            group.Index -= 1;
        }

        await context.SaveChangesAsync();
    }

    public async Task<bool> InformationTypeMove(InformationTypeMoveDto moveDto)
    {
        var item = await context.InformationTypes
            .NotDeleted()
            .FirstOrDefaultAsync(x => x.Id == moveDto.Id);

        if (item == null) return false;

        var fromIndex = item.Index;
        var toIndex = moveDto.ToIndex;

        if (fromIndex == toIndex) return true;
        if (toIndex < 0) return false;

        // Determine the max index from both InformationTypes and InformationGroups
        var maxTypeIndex = await context.InformationTypes
            .NotDeleted()
            .Select(x => (int?)x.Index)
            .MaxAsync() ?? -1;

        var maxGroupIndex = await context.InformationGroups
            .Select(x => (int?)x.Index)
            .MaxAsync() ?? -1;

        var maxIndex = Math.Max(maxTypeIndex, maxGroupIndex);

        if (toIndex > maxIndex)
        {
            toIndex = maxIndex;
        }

        if (fromIndex < toIndex)
        {
            await ShiftIndexesDown(fromIndex, toIndex);
        }
        else
        {
            await ShiftIndexesUp(toIndex, fromIndex);
        }

        item.Index = toIndex;

        await context.SaveChangesAsync();
        return true;
    }

    private async Task ShiftIndexesDown(int fromIndex, int toIndex)
    {
        var affectedTypes = await context.InformationTypes
            .NotDeleted()
            .Where(x => x.Index > fromIndex && x.Index <= toIndex)
            .ToListAsync();

        var affectedGroups = await context.InformationGroups
            .Where(x => x.Index > fromIndex && x.Index <= toIndex)
            .ToListAsync();

        foreach (var type in affectedTypes)
        {
            type.Index -= 1;
        }

        foreach (var group in affectedGroups)
        {
            group.Index -= 1;
        }
    }

    private async Task ShiftIndexesUp(int toIndex, int fromIndex)
    {
        var affectedTypes = await context.InformationTypes
            .NotDeleted()
            .Where(x => x.Index >= toIndex && x.Index < fromIndex)
            .ToListAsync();

        var affectedGroups = await context.InformationGroups
            .Where(x => x.Index >= toIndex && x.Index < fromIndex)
            .ToListAsync();

        foreach (var type in affectedTypes)
        {
            type.Index += 1;
        }

        foreach (var group in affectedGroups)
        {
            group.Index += 1;
        }
    }


    public async Task<List<InformationTypeReturnDto>> GetAllInformationTypes()
    {
        return await context.InformationTypes
            .NotDeleted()
            .ProjectTo<InformationTypeReturnDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<InformationTypeReturnDto?> GetInformationTypeById(int id)
    {
        return await context.InformationTypes
            .Where(x => x.Id == id)
            .NotDeleted()
            .ProjectTo<InformationTypeReturnDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }


    //TODO 
    public async Task<InformationGroup> SaveInformationGroup(InformationGroupCreateDto createDto)
    {
        var group = mapper.Map<InformationGroup>(createDto);
        context.InformationGroups.Add(group);

        var itemsToUpdate = context.InformationTypes
            .NotDeleted()
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