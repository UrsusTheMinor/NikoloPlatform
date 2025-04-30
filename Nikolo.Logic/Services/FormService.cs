using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nikolo.Data;
using Nikolo.Data.DTOs.InformationForm;
using Nikolo.Data.DTOs.InformationForm.Group;
using Nikolo.Data.DTOs.InformationForm.Type;
using Nikolo.Data.Extensions;
using Nikolo.Data.Models.Form;
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

        var itemsToUpdate = new List<InformationItem>();
        itemsToUpdate.AddRange(await context.InformationTypes
            .NotDeleted()
            .Where(i => i.Group == group)
            .ToListAsync());


        if (group == null)
        {
            var groups = await context.InformationGroups.ToListAsync();
            itemsToUpdate.AddRange(groups);
        }

        context.InformationTypes.Add(infoType);
        itemsToUpdate.Insert(createDto.Index, infoType);

        for (int i = 0; i < itemsToUpdate.Count; i++)
        {
            itemsToUpdate[i].Index = i;
        }


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
            infoType.Name = editDto.TypeName;
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

        var itemsToUpdate = new List<InformationItem>();
        itemsToUpdate.AddRange(await context.InformationTypes
            .NotDeleted()
            .Where(i => i.Group == infoType.Group)
            .ToListAsync());


        if (infoType.Group == null)
        {
            var groups = await context.InformationGroups.ToListAsync();
            itemsToUpdate.AddRange(groups);
        }

        itemsToUpdate.Remove(infoType);

        infoType.DeletedOn = DateTime.Now;
        infoType.Group = null;
        infoType.Index = -1;

        for (int i = 0; i < itemsToUpdate.Count; i++)
        {
            itemsToUpdate[i].Index = i;
        }

        await context.SaveChangesAsync();
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

    public async Task<InformationGroup> SaveInformationGroup(InformationGroupCreateDto createDto)
    {
        var infoGroup = mapper.Map<InformationGroup>(createDto);

        var itemsToUpdate = new List<InformationItem>();
        itemsToUpdate.AddRange(await context.InformationTypes
            .NotDeleted()
            .Where(i => i.Group == null)
            .ToListAsync());


        var groups = await context.InformationGroups.ToListAsync();
        itemsToUpdate.AddRange(groups);


        context.InformationGroups.Add(infoGroup);
        itemsToUpdate.Insert(createDto.Index, infoGroup);

        for (int i = 0; i < itemsToUpdate.Count; i++)
        {
            itemsToUpdate[i].Index = i;
        }


        await context.SaveChangesAsync();
        return infoGroup;
    }

    public async Task<InformationGroup?> EditInformationGroup(InformationGroupEditDto editDto)
    {
        var infoGroup = await context.InformationGroups.FindAsync(editDto.Id);

        if (infoGroup == null)
        {
            return null;
        }

        if (!string.IsNullOrWhiteSpace(editDto.Name))
        {
            infoGroup.Name = editDto.Name;
        }

        if (editDto.ElementsPerRow.HasValue)
        {
            infoGroup.ElementsPerRow = editDto.ElementsPerRow.Value;
        }

        await context.SaveChangesAsync();

        return infoGroup;
    }

    public async Task DeleteInformationGroup(int id)
    {
        var group = await context.InformationGroups.FindAsync(id);

        if (group == null)
        {
            return;
        }

        var itemsToUpdate = new List<InformationItem>();
        itemsToUpdate.AddRange(await context.InformationTypes
            .NotDeleted()
            .Where(i => i.Group == null)
            .ToListAsync());

        var groups = await context.InformationGroups.ToListAsync();
        itemsToUpdate.AddRange(groups);

        if (group.InformationTypes != null)
        {
            foreach (var type in group.InformationTypes)
            {
                itemsToUpdate.Insert(group.Index + group.InformationTypes.IndexOf(type), type);
                type.Group = null;
            }
        }


        for (int i = 0; i < itemsToUpdate.Count; i++)
        {
            itemsToUpdate[i].Index = i;
        }

        await context.SaveChangesAsync();
    }

    public async Task<List<InformationGroupReturnDto>> GetAllInformationGroups()
    {
        return await context.InformationGroups
            .ProjectTo<InformationGroupReturnDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<InformationGroupReturnDto?> GetInformationGroupById(int id)
    {
        return await context.InformationGroups
            .Where(x => x.Id == id)
            .ProjectTo<InformationGroupReturnDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    // public int FromIndex { get; set; }
    // public int ToIndex { get; set; }
    // public int? TargetGroupId { get; set; } // group to move the item to
    // public int? CurrentGroupId { get; set; }
    public async Task<bool> Move(MoveDto moveDto)
    {
        var moveType = await context.InformationTypes
            .FirstOrDefaultAsync(x =>
                x.Index == moveDto.FromIndex
                || (x.Group == null && moveDto.CurrentGroupId == null)
                || (x.Group != null && x.Group.Id == moveDto.CurrentGroupId));


        var moveGroup = await context.InformationGroups
            .FirstOrDefaultAsync(x => x.Index == moveDto.FromIndex);


        if (moveGroup == null && moveType == null)
        {
            return false;
        }

        var currentScope = new List<InformationItem>();
        var targetScope = new List<InformationItem>();

        // InformationType
        if (moveType != null)
        {
            currentScope.AddRange(await context.InformationTypes
                .Where(x =>
                    (x.Group == null && moveDto.CurrentGroupId == null)
                    || (x.Group != null && x.Group.Id == moveDto.CurrentGroupId))
                .ToListAsync());
            
            currentScope.AddRange(await context.InformationGroups.ToListAsync());
            currentScope.Remove(moveType);

            if (moveDto.TargetGroupId != moveDto.CurrentGroupId)
            {
                targetScope.AddRange(await context.InformationTypes
                    .Where(x => x.Group != null && x.Group.Id == moveDto.TargetGroupId)
                    .ToListAsync());
            }
            else
            {
                InformationItem[] currentItems = [];
                currentScope.CopyTo(currentItems);
                targetScope.AddRange(currentItems);
            }
            
            targetScope = targetScope.OrderBy(x => x.Index).ToList();
            currentScope = currentScope.OrderBy(x => x.Index).ToList();
            targetScope.Insert(moveDto.ToIndex, moveType);
            
            
            for (int i = 0; i < targetScope.Count; i++)
            {
                targetScope[i].Index = i;
            }
            
            for (int i = 0; i < currentScope.Count; i++)
            {
                currentScope[i].Index = i;
            }

            return true;
        }

        if (moveDto.TargetGroupId != null || moveDto.CurrentGroupId != null)
        {
            return false;
        }
        
        // InformatinoGroup
        
    }
}