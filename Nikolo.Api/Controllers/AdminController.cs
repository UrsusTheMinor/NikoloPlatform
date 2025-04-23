using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Nikolo.Data.DTOs.InformationForm;
using Nikolo.Data.Models;
using Nikolo.Logic.Contracts;

namespace Nikolo.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AdminController(IMapper mapper, IFormService formService)
    : ControllerBase
{
    private readonly IMapper mapper = mapper;

    [HttpPost]
    [Authorize("form:edit")]
    public async Task<IActionResult> AddInformationType([FromBody] InformationTypeCreateDto createDto)
    {
        var infoType = await formService.SaveInformationType(createDto);
        return Created(string.Empty, new { Message = "InformationType saved successfully", InformationType = infoType });
    }
    
    // TODO: InformationType: Delete, Move, Get
    [HttpPut]
    [Authorize("form:edit")]
    public async Task<IActionResult> EditInformationType([FromBody] InformationTypeEditDto editDto)
    {
        var infoType = await formService.EditInformationType(editDto);

        if (infoType == null)
        {
            return BadRequest();
        }
        
        return Created(string.Empty, new { Message = "InformationType saved successfully", InformationType = infoType });
    }

    [HttpDelete("{id:int}")]
    [Authorize("form:delete")]
    public async Task<IActionResult> DeleteInformationType([FromRoute] int id)
    {
        await formService.DeleteInformationType(id);
        return NoContent();
    }

    [HttpPut("move")]
    [Authorize("form:edit")]
    public async Task<IActionResult> MoveInformationType([FromBody] InformationTypeMoveDto moveDto)
    {
        var result = await formService.InformationTypeMove(moveDto);

        if (!result)
        {
            return BadRequest();
        }

        return Ok("InformationType successfully moved");
    }

    [HttpGet]
    [Authorize("form:edit")]
    public async Task<ActionResult<List<InformationTypeReturnDto>>> GetAllInformationTypes()
    {
        return await formService.GetAllInformationTypes();
    }

    [HttpGet("{id:int}")]
    [Authorize("form:edit")]
    public async Task<InformationTypeReturnDto?> GetInformationTypeById([FromRoute] int id)
    {
        return await formService.GetInformationTypeById(id);
    }
    
    // TODO: InformationGroup: Edit, Delete
    
    [HttpPost]
    [Authorize("form:edit")]
    public async Task<IActionResult> AddInformationGroup([FromBody] InformationGroupCreateDto createDto)
    {
        var infoGroup = await formService.SaveInformationGroup(createDto);
        return Created(string.Empty, new { Message = "InformationGroup added successfully", InformationGroup = infoGroup });
    }
}