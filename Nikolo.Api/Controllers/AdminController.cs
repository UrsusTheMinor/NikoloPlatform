using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    
    //TODO: InformationType: Edit, Delete, Move
    
    //TODO: InformationGroup: Edit, Delete
    
    [HttpPost]
    [Authorize("form:edit")]
    public async Task<IActionResult> AddInformationGroup([FromBody] InformationGroupCreateDto createDto)
    {
        var infoGroup = await formService.SaveInformationGroup(createDto);
        return Created(string.Empty, new { Message = "InformationGroup added successfully", InformationGroup = infoGroup });
    }
}