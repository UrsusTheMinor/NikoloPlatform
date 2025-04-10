using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nikolo.Data.DTOs.Skill;
using Nikolo.Logic.Contracts;

namespace Nikolo.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SkillsController(ISkillService skillService, IMapper mapper)
    : ControllerBase
{
    
    private readonly IMapper mapper = mapper;

    [HttpPost(Name = nameof(CreateSkill))]
    [Authorize("skills:create")]
    public async Task<IActionResult> CreateSkill([FromBody] CreateSkillDto skillDto)
    {
        var skill = await skillService.CreateSkill(skillDto);
        return Created(string.Empty, new { Message = "User saved successfully", Skill = skill });
    }
    
    [HttpGet(Name = nameof(GetAllSkills))]
    [Authorize]
    public async Task<ActionResult<List<SkillDto>>> GetAllSkills()
    {
        var skills = await skillService.GetAllSkills();
        return Ok(mapper.Map<List<SkillDto>>(skills));
    }

    [HttpDelete("{skillId:int}", Name = nameof(DeleteSkill))]
    [Authorize("skills:delete")]
    public async Task<IActionResult> DeleteSkill([FromRoute] int skillId)
    {
        await skillService.DeleteSkill(skillId);
        return NoContent();
    }
    
    
}