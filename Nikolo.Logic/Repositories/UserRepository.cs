using Microsoft.EntityFrameworkCore;
using Nikolo.Api.DTOs;
using Nikolo.Data;
using Nikolo.Data.Models;
using Nikolo.Logic.Contracts;

namespace Nikolo.Logic.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    private readonly ApplicationDbContext _context = context;
    
    public async Task CreateOrUpdateUser(UserUpsertDto dto)
    {
        // Check if user exists (based on Auth0Id)
        var user = await _context.Employees.FirstOrDefaultAsync(u => u.Auth0Id == dto.Auth0Id);
        if (user == null)
        {
            user = new Employee
            {
                Auth0Id = dto.Auth0Id,
                Email = dto.Email,
                Name = dto.Name,
                CreatedAt = DateTime.UtcNow
            };
            _context.Employees.Add(user);
        }
        else
        {
            // Update fields as needed
            user.Email = dto.Email;
            user.Name = dto.Name;
        }
        await _context.SaveChangesAsync();
    }
}