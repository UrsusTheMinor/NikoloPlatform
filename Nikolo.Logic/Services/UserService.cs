using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nikolo.Data;
using Nikolo.Data.Models;
using Nikolo.Logic.Contracts;

namespace Nikolo.Logic.Services;

public class UserService(ApplicationDbContext context, ILogger<UserService> logger, ISkillService skillService) : IUserRepository
{
    private readonly ILogger <UserService> logger = logger;
    
    public async Task<Employee> CreateUser(string auth0Id)
    {
        var existingUser = await GetUser(auth0Id);

        if (existingUser != null)
        {
            logger.LogWarning("User with auth0 ID {Auth0Id} already exists.", auth0Id);
            return existingUser;
        }

        var user = new Employee
        {
            Auth0Id = auth0Id,
            CreatedAt = DateTime.UtcNow
        };

        context.Employees.Add(user);
        await context.SaveChangesAsync();
        logger.LogInformation("User with auth0 ID {Auth0Id} has been created.", auth0Id);
        return user;
    }

    public async Task<Employee?> GetUser(string auth0Id)
    {
        logger.LogInformation("Getting user {Auth0Id}", auth0Id);
        return await context.Employees.FirstOrDefaultAsync(x => x.Auth0Id == auth0Id);
    }

    public async Task AddBuddy(Employee user, Employee buddy)
    {
        bool exists = await context.TeamBuddies.AnyAsync(tb =>
            (tb.Employee1Id == user.Id && tb.Employee2Id == buddy.Id) ||
            (tb.Employee1Id == buddy.Id && tb.Employee2Id == user.Id));
        
        logger.LogInformation("{UserAuth0Id} and buddy {BuddyAuth0Id} exist: {Existence}", user.Auth0Id, buddy.Auth0Id, exists);
        
        if (!exists)
        {
            var teamBuddy = new TeamBuddy
            {
                Employee1Id = user.Id,
                Employee2Id = buddy.Id,
                Employee1 = user,
                Employee2 = buddy
            };
            await context.TeamBuddies.AddAsync(teamBuddy);
            await context.SaveChangesAsync();
            logger.LogInformation("Successfully added new Buddy connection");
        }
    }

    public async Task<bool> AddSkill(Employee user, int skillId)
    {
        var skill = await skillService.GetSkill(skillId);

        if (skill == null)
        {
            logger.LogWarning("Skill with id {SkillId} was not found", skillId);
            return false;
        }

        bool exists = await context.SkillEmployees.AnyAsync(se =>
            (se.EmployeeId == user.Id && se.SkillId == skill.Id));

        logger.LogInformation("Connection between {UserAuth0Id} and skill {SkillId} exist: {Existence}", user.Auth0Id,
            skill.Id, exists);

        if (exists)
        {
            return false;
        }

        var skillEmployee = new SkillEmployee()
        {
            EmployeeId = user.Id,
            SkillId = skill.Id,
            Skill = skill,
            Employee = user,
        };

        await context.SkillEmployees.AddAsync(skillEmployee);
        await context.SaveChangesAsync();
        logger.LogInformation("Successfully added new Skill connection");
        return true;
    }

    public async Task<List<Skill>> GetSkills(Employee user)
    {
        return await context.SkillEmployees.Where(se => se.EmployeeId == user.Id).Select(se => se.Skill).ToListAsync();
    }

    public async Task RemoveSkill(Employee user, int skillId)
    {
        var skill = await context.Skills.FirstOrDefaultAsync(se => se.Id == skillId);
        if (skill == null)
        {
            return;
        }
        
        var skillemployee = await context.SkillEmployees.FirstOrDefaultAsync(se => se.Employee == user && se.Skill == skill);
        if (skillemployee == null)
        {
            return;
        }
        
        context.SkillEmployees.Remove(skillemployee);
        await context.SaveChangesAsync();
    }
}