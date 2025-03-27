using Microsoft.AspNetCore.Mvc;
using Nikolo.Api.DTOs;
using Nikolo.Logic.Contracts;

namespace Nikolo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserRepository userRepository, IConfiguration configuration)
    : ControllerBase
{
    [HttpPost("upsert")]
    public async Task<IActionResult> UpsertUser([FromBody] UserUpsertDto dto)
    {
        // Verify the API key from header (for extra security)
        if (!Request.Headers.TryGetValue("x-api-key", out var apiKey) ||
            apiKey != configuration["Auth0:UpsertApiKey"])
        {
            return Unauthorized();
        }

        await userRepository.CreateOrUpdateUser(dto);
        
        return Ok();
    }
}