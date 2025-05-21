using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Nikolo.Api.AutoMapperProfiles;
using Nikolo.Data;
using Nikolo.Data.DTOs.InformationForm.Group;
using Nikolo.Data.DTOs.InformationForm.Type;
using Nikolo.Data.Models.Form;
using Nikolo.Logic.Services;

namespace Nikolo.Test.Cases;

public class FormTests
{
    private static ApplicationDbContext GetInMemoryDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        // You can mock IConfiguration if needed, or pass null if unused
        var configuration = new ConfigurationBuilder().Build(); // or use a mock if you access it

        return new ApplicationDbContext(options, configuration);
    }

    private static ILogger<FormService> GetMockLogger()
    {
        var mock = new Mock<ILogger<FormService>>();
        ILogger<FormService> logger = mock.Object;
        return logger;
    }

    private static IMapper GetMockMapper()
    {
        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AvailableTimeMappingProfile());
            cfg.AddProfile(new FormMappingProfile());
            cfg.AddProfile(new SkillMappingProfile());
        });
        var mapper = mockMapper.CreateMapper();
        return mapper;
    }

    [Fact]
    public async Task SaveInformationType_NoGroup_Add()
    {
        // Arrange
        var context = GetInMemoryDbContext("SaveUserTestDb1");
        var logger = GetMockLogger();
        var mapper = GetMockMapper();
        var formService = new FormService(context, logger, mapper);

        var user = new InformationTypeCreateDto()
        {
            FieldType = "string",
            GroupId = null,
            Index = 0,
            TypeName = "Test 1"
        };

        // Act
        await formService.SaveInformationType(user); // Assume this method exists and saves to context

        // Assert
        var savedUser = await context.InformationTypes.FirstOrDefaultAsync(u => u.Name == user.TypeName);
        Assert.NotNull(savedUser);
        Assert.Equal(user.TypeName, savedUser.Name);
        
        // starts with 1
        Assert.Equal(1, savedUser.Id);
    }

    [Fact]
    public async Task SaveInformationType_NoGroup_AddMultiple_NoIndexTest()
    {
        // Arrange
        var context = GetInMemoryDbContext("SaveUserTestDb2");
        var logger = GetMockLogger();
        var mapper = GetMockMapper();
        var formService = new FormService(context, logger, mapper);

        var user1 = new InformationTypeCreateDto()
        {
            FieldType = "string",
            GroupId = null,
            Index = 0,
            TypeName = "Test 1"
        };

        var user2 = new InformationTypeCreateDto()
        {
            FieldType = "string",
            GroupId = null,
            Index = 1,
            TypeName = "Test 2"
        };
        
        // Act
        await formService.SaveInformationType(user1); // Assume this method exists and saves to context
        await formService.SaveInformationType(user2);

        // Assert
        var savedUser1 = await context.InformationTypes.FirstOrDefaultAsync(u => u.Name == user1.TypeName);
        Assert.NotNull(savedUser1);
        Assert.Equal(user1.TypeName, savedUser1.Name);
        
        var savedUser2 = await context.InformationTypes.FirstOrDefaultAsync(u => u.Name == user2.TypeName);
        Assert.NotNull(savedUser2);
        Assert.Equal(user2.TypeName, savedUser2.Name);
        
        // starts with 1
        Assert.Equal(1, savedUser1.Id);
        Assert.Equal(2, savedUser2.Id);
        
        Assert.Equal(user1.Index, savedUser1.Index);
        Assert.Equal(user2.Index, savedUser2.Index);
    }

    [Fact]
    public async Task SaveInformationType_NoGroup_IndexTest()
    {
        // Arrange
        var context = GetInMemoryDbContext("SaveUserTestDb3");
        var logger = GetMockLogger();
        var mapper = GetMockMapper();
        var formService = new FormService(context, logger, mapper);

        var user1 = new InformationTypeCreateDto()
        {
            FieldType = "string",
            GroupId = null,
            Index = 0,
            TypeName = "Test 1"
        };

        var user2 = new InformationTypeCreateDto()
        {
            FieldType = "string",
            GroupId = null,
            Index = 0,
            TypeName = "Test 2"
        };
        
        // Act
        await formService.SaveInformationType(user1); // Assume this method exists and saves to context
        await formService.SaveInformationType(user2);
        
        // Assert
        var savedUser1 = await context.InformationTypes.FirstOrDefaultAsync(u => u.Name == user1.TypeName);
        Assert.NotNull(savedUser1);
        
        var savedUser2 = await context.InformationTypes.FirstOrDefaultAsync(u => u.Name == user2.TypeName);
        Assert.NotNull(savedUser2);
        
        Assert.NotEqual(user1.Index, savedUser1.Index);
        
        Assert.Equal(1, savedUser1.Index);
        Assert.Equal(0, savedUser2.Index);
    }

    [Fact]
    public async Task SaveInformationGroup_Add()
    {
        // Arrange
        var context = GetInMemoryDbContext("SaveGroupTestDb1");
        var logger = GetMockLogger();
        var mapper = GetMockMapper();
        var formService = new FormService(context, logger, mapper);


        var group = new InformationGroupCreateDto()
        {
            Name = "Group 1",
            Index = 0
        };

        // Act
        await formService.SaveInformationGroup(group); // Assume this method exists and saves to context

        // Assert
        var savedUser = await context.InformationGroups.FirstOrDefaultAsync(g => g.Name == group.Name);
        Assert.NotNull(savedUser);
        Assert.Equal(group.Name, savedUser.Name);
        
        // starts with 1
        Assert.Equal(1, savedUser.Id);
    }

    [Fact]
    public async Task SaveInformationGroup_IndexTest()
    {
        // Arrange
        var context = GetInMemoryDbContext("SaveGroupTestDb1");
        var logger = GetMockLogger();
        var mapper = GetMockMapper();
        var formService = new FormService(context, logger, mapper);

        var group = new InformationGroupCreateDto()
        {
            Name = "Group 1",
            Index = 0
        };

        var group2 = new InformationGroupCreateDto()
        {
            Name = "Group 2",
            Index = 0
        };

        var group3 = new InformationGroupCreateDto()
        {
            Name = "Group 3",
            Index = 0
        };
        
        // Act
        await formService.SaveInformationGroup(group); // Assume this method exists and saves to context
        await formService.SaveInformationGroup(group2);
        await formService.SaveInformationGroup(group3);
        
        var savedGroup1 = await context.InformationGroups.FirstOrDefaultAsync(g => g.Name == group.Name);
        var savedGroup2 = await context.InformationGroups.FirstOrDefaultAsync(g => g.Name == group2.Name);
        var savedGroup3 = await context.InformationGroups.FirstOrDefaultAsync(g => g.Name == group3.Name);
        
        Assert.NotNull(savedGroup1);
        Assert.NotNull(savedGroup2);
        Assert.NotNull(savedGroup3);
        
        Assert.NotEqual(group.Index, savedGroup1.Index);
        Assert.NotEqual(group2.Index, savedGroup2.Index);
        Assert.Equal(group3.Index, savedGroup3.Index);
        
        Assert.Equal(0, savedGroup3.Index);
        Assert.Equal(1, savedGroup2.Index);
        Assert.Equal(2, savedGroup1.Index);
    }
    
}