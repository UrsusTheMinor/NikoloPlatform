using AutoMapper;
using AutoMapper.QueryableExtensions;
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

    public async Task<InformationType?> EditInformationType(InformationTypeEditDto editDto)
    {
        var infoType = await context.InformationTypes.FirstOrDefaultAsync(x => x.Id == editDto.Id);

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

        await context.SaveChangesAsync();
        return infoType;
    }

    public async Task DeleteInformationType(int id)
    {
        var infoType = await context.InformationTypes.FirstOrDefaultAsync(x => x.Id == id);

        if (infoType == null)
        {
            return;
        }
        
        context.InformationTypes.Remove(infoType);
        await context.SaveChangesAsync();
    }

    // WARN : I HAVE NO IDEA IF THIS METHODS WORKS (CHATGPT MAGIC), what i have checked it should work
    public async Task<bool> InformationTypeMove(InformationTypeMoveDto moveDto)
    {
        // Assuming you have a DbContext named _context and a DbSet<InformationType> named InformationTypes
        var item = await context.InformationTypes.FirstOrDefaultAsync(x => x.Id == moveDto.Id);
        if (item == null)
            return false;

        int fromIndex = item.Index;
        int toIndex = moveDto.ToIndex;

        if (fromIndex == toIndex)
            return true; // No movement needed

        if (fromIndex < toIndex)
        {
            // Moving down: decrease index of items between fromIndex+1 and toIndex
            var affected = await context.InformationTypes
                .Where(x => x.Index > fromIndex && x.Index <= toIndex)
                .ToListAsync();

            foreach (var info in affected)
            {
                info.Index -= 1;
            }
        }
        else
        {
            // Moving up: increase index of items between toIndex and fromIndex-1
            var affected = await context.InformationTypes
                .Where(x => x.Index >= toIndex && x.Index < fromIndex)
                .ToListAsync();

            foreach (var info in affected)
            {
                info.Index += 1;
            }
        }

        item.Index = toIndex;

        await context.SaveChangesAsync();

        return true;
    }

    public async Task<List<InformationTypeReturnDto>> GetAllInformationTypes()
    {
        return await context.InformationTypes
            .ProjectTo<InformationTypeReturnDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<InformationTypeReturnDto?> GetInformationTypeById(int id)
    {
        return await context.InformationTypes
            .Where(x => x.Id == id)
            .ProjectTo<InformationTypeReturnDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
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