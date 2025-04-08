using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nikolo.Api.Responses.User;
using Nikolo.Data.DTOs.Skill;
using Nikolo.Data.DTOs.User;
using Nikolo.Logic.Contracts;

namespace Nikolo.Api.Controllers;

/// <summary>
/// Handles user related actions
/// </summary>
/// <dependency><see cref="IUserService"/></dependency>
[ApiController]
[Route("[controller]")]
public class UsersController(IUserService userService, IUserTimeService userTimeService, IMapper mapper)
    : ControllerBase
{
    private readonly IMapper mapper = mapper;
    
    /// <summary>
    /// Saves a user from the JWT claim if they do not already exist in the system.
    /// </summary>
    /// <remarks>
    /// This endpoint extracts the Auth0 user ID ("sub" claim) from the JWT token 
    /// and attempts to store it in the database. If the user already exists, no action is taken.
    /// </remarks>
    /// <returns>
    /// A response indicating whether the user was successfully saved.
    /// </returns>
    /// <response code="201">User successfully added.</response>
    /// <response code="401">Unauthorized - No valid Auth0 ID found.</response>
    [Authorize]
    [HttpPost("", Name = nameof(SaveUser))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserCreatedResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = null!)]
    public async Task<IActionResult> SaveUser()
    {
        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // "sub" claim in JWT

        if (string.IsNullOrEmpty(auth0Id))
        {
            return Unauthorized("Auth0ID not found.");
        }
        
        var user= await userService.CreateUser(auth0Id);
        
        return Created(string.Empty, new { Message = "User saved successfully", User = user });
    }

    /// <summary>
    /// Signs the logged-in user up for a skill.
    /// </summary>
    /// <remarks>
    /// This endpoint extracts the Auth0 user ID ("sub" claim) from the JWT token
    /// and uses it to register a skill to this user
    /// </remarks>
    /// <returns>
    /// A response indicating whether the user was successfully saved.
    /// </returns>
    /// <response code="201">User was not enlisted in the database and was added alongside with assigning a skill to it.</response>
    /// <response code="200">User was successfully signed up for skill.</response>
    /// <response code="401">Unauthorized - No valid Auth0 ID found.</response>
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserCreatedResponse))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = null!)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = null!)]
    [HttpPost("skill", Name = nameof(SignUpForSkill))]
    [Authorize]
    public async Task<IActionResult> SignUpForSkill([FromBody] SignUpForSkillDto skillDto)
    {
        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(auth0Id))
        {
            return Unauthorized("Auth0 ID not found.");
        }

        var createdUser = false;
        var user = await userService.GetUser(auth0Id);

        if (user == null)
        {
            user = await userService.CreateUser(auth0Id);
            createdUser = true;
        }

        await userService.AddSkillToUser(user, skillDto.SkillId);
        return createdUser ? Created(string.Empty, new { Message = "User saved successfully", User = user }) : Ok();
    }

    /// <summary>
    /// Gets the skills of the logged-in user.
    /// </summary>
    /// <remarks>
    /// This endpoint extracts the Auth0 user ID ("sub" claim) from the JWT token
    /// and uses it to get all skill of this user
    /// </remarks>
    /// <returns>
    /// A List of <see cref="SkillDto"/> which the user has registered to him.
    /// </returns>
    /// <response code="201">User was not enlisted in the database and was added
    /// alongside with getting his registered skill to it.</response>
    /// <response code="200">Successfully returned a List of <see cref="SkillDto"/> containing the users registered skills.</response>
    /// <response code="401">Unauthorized - No valid Auth0 ID found.</response>
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(List<SkillDto>))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SkillDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = null!)]
    [HttpGet("skill", Name = nameof(GetSkills))]
    [Authorize]
    public async Task<ActionResult<List<SkillDto>>> GetSkills()
    {
        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(auth0Id))
        {
            return Unauthorized("Auth0 ID not found.");
        }

        var createdUser = false;
        var user = await userService.GetUser(auth0Id);

        if (user == null)
        {
            user = await userService.CreateUser(auth0Id);
            createdUser = true;
        }
        
        var skills = await userService.GetSkillsFromUser(user);

        var skillDtos = mapper.Map<List<SkillDto>>(skills);
        
        return createdUser
            ? Created(string.Empty, skillDtos)
            : Ok(skillDtos);
    }

    /// <summary>
    /// Removes a skill of the logged-in user.
    /// </summary>
    /// <remarks>
    /// This endpoint extracts the Auth0 user ID ("sub" claim) from the JWT token
    /// and uses it to remove a skill from an user.
    /// </remarks>
    /// <returns>
    /// A response indicating whether the user was successfully saved or the delete quietly passed.
    /// </returns>
    /// <response code="201">User was not enlisted in the database and was added</response>
    /// <response code="200">Successfully removed a skill.</response>
    /// <response code="401">Unauthorized - No valid Auth0 ID found.</response>
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserCreatedResponse))]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = null!)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = null!)]
    [HttpDelete("skill/{skillid}", Name = nameof(RemoveSkill))]
    [Authorize]
    public async Task<IActionResult> RemoveSkill([FromRoute] int skillId)
    {
        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(auth0Id))
        {
            return Unauthorized("Auth0 ID not found.");
        }

        var user = await userService.GetUser(auth0Id);

        if (user == null)
        {
            user = await userService.CreateUser(auth0Id);
            return Created(string.Empty, new { Message = "User saved successfully", User = user });
        }
        
        await userService.RemoveSkillFromUser(user, skillId);
        return NoContent();
    }

    
    /// <summary>
    /// Adds a Time to an user
    /// </summary>
    /// <remarks>
    /// This endpoint extracts the Auth0 user ID ("sub" claim) from the JWT token
    /// and uses it to add a skill to an user, if no user is yet created it will be created
    /// along the way
    /// </remarks>
    /// <returns>
    /// A Response which includes the used User, and the created Time, if not null
    /// </returns>
    /// <response code="201">Time was successfully added, can be possible that user was created</response>
    /// <response code="400">Wrong arguments were used, probably was the starttime after the endtime
    /// or startime overlaps two other times</response>
    /// <response code="401">Unauthorized - No valid Auth0 ID found.</response>
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TimeAddResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = null!)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [HttpPost("time", Name = nameof(AddAvailableTime))]
    [Authorize]
    public async Task<IActionResult> AddAvailableTime([FromBody] AvailableTimeCreateDto availableTimeCreateDto)
    {
        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(auth0Id))
        {
            return Unauthorized("Auth0 ID not found.");
        }
        
        var createdUser = false;
        var user = await userService.GetUser(auth0Id);

        if (user == null)
        {
            user = await userService.CreateUser(auth0Id);
            createdUser = true;
        }
        
        var time = await userTimeService.AddTime(user, availableTimeCreateDto);
        
        if (time == null)
        {
            return BadRequest("Please check your arguments, something went wrong.");
        }

        var createdUserText = createdUser ? "and created user along the way" : "";
        
        return Created(string.Empty, new {Message = $"Successfully added time to user {createdUserText}", Time = time, User = user });
    }

    /// <summary>
    /// Edits a time of an user
    /// </summary>
    /// <remarks>
    /// This endpoint extracts the Auth0 user ID ("sub" claim) from the JWT token
    /// and uses it to edit a time of an user, using the <see cref="AvailableTimeEditDto"/> from the body, if no user is yet created it will be created
    /// along the way
    /// </remarks>
    /// <returns>
    /// A response indicating whether the user was successfully saved or the edit quietly passed
    /// </returns>
    /// <response code="201">User was not yet created, but was along the execution</response>
    /// <response code="400">Time was not yet created </response>
    /// <response code="200">Time was successfully edited</response>
    /// <response code="401">Unauthorized - No valid Auth0 ID found.</response>
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserCreatedResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = null!)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = null!)]
    [Authorize]
    [HttpPut("time", Name = nameof(EditAvailableTime))]
    public async Task<IActionResult> EditAvailableTime([FromBody] AvailableTimeEditDto availableTimeEditDto)
    {
        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(auth0Id))
        {
            return Unauthorized("Auth0 ID not found.");
        }

        var user = await userService.GetUser(auth0Id);

        if (user == null)
        {
            user = await userService.CreateUser(auth0Id);
            return Created(string.Empty, new { Message = "User saved successfully", User = user });
        }

        var time = await userTimeService.EditTime(user, availableTimeEditDto);

        if (time == null)
        {
            return BadRequest("This time was not yet created");
        }

        return Ok();
    }

    /// <summary>
    /// Get the times of an user
    /// </summary>
    /// <remarks>
    /// This endpoint extracts the Auth0 user ID ("sub" claim) from the JWT token
    /// and uses it to return all time of an user, if no user is yet created it will be created along the way
    /// </remarks>
    /// <returns>
    /// A List of the Available Times the User has registered
    /// </returns>
    /// <response code="201">User was not yet created, but was along the execution</response>
    /// <response code="200">Successfully passed</response>
    /// <response code="401">Unauthorized - No valid Auth0 ID found.</response>
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserCreatedResponse))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AvailableTimeReturnDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = null!)]
    [Authorize]
    [HttpGet("time", Name = nameof(GetAvailableTimes))]
    public async Task<ActionResult<List<AvailableTimeReturnDto>>> GetAvailableTimes()
    {
        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(auth0Id))
        {
            return Unauthorized("Auth0 ID not found.");
        }

        var user = await userService.GetUser(auth0Id);

        if (user == null)
        {
            user = await userService.CreateUser(auth0Id);
            return Created(string.Empty, new { Message = "User saved successfully", User = user });
        }
        
        var times = userTimeService.GetAvailableTimes(user);
        return Ok(mapper.Map<AvailableTimeReturnDto>(times));
    }
    
    /// <summary>
    /// Get the times of an user of a given day
    /// </summary>
    /// <remarks>
    /// This endpoint extracts the Auth0 user ID ("sub" claim) from the JWT token
    /// and uses it to return all time of an user of a specific date, if no user is yet created it will be created along the way
    /// </remarks>
    /// <returns>
    /// A List of the Available Times the User has registered to a specific date
    /// </returns>
    /// <response code="201">User was not yet created, but was along the execution</response>
    /// <response code="200">Successfully passed</response>
    /// <response code="401">Unauthorized - No valid Auth0 ID found.</response>
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserCreatedResponse))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AvailableTimeReturnDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = null!)]
    [Authorize]
    [HttpGet("time/{date}", Name = nameof(GetAvailableTimesByDate))]
    public async Task<ActionResult<List<AvailableTimeReturnDto>>> GetAvailableTimesByDate([FromRoute] DateOnly date)
    {
        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(auth0Id))
        {
            return Unauthorized("Auth0 ID not found.");
        }

        var user = await userService.GetUser(auth0Id);

        if (user == null)
        {
            user = await userService.CreateUser(auth0Id);
            return Created(string.Empty, new { Message = "User saved successfully", User = user });
        }
        
        var times = userTimeService.GetAvailableTimesForDay(user, date);
        return Ok(mapper.Map<AvailableTimeReturnDto>(times));
    }
}