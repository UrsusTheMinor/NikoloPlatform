using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Nikolo.Data.DTOs.InformationForm;
using Nikolo.Data.DTOs.InformationForm.Group;
using Nikolo.Logic.Contracts;

namespace Nikolo.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class InformationGroupController(IMapper mapper, IFormService formService)
    : ControllerBase
{
    private readonly IMapper mapper = mapper;
    
    // TODO: InformationGroup: Edit, Delete
    
    [HttpPost]
    [Authorize("form:edit")]
    public async Task<IActionResult> AddInformationGroup([FromBody] InformationGroupCreateDto createDto)
    {
        var infoGroup = await formService.SaveInformationGroup(createDto);
        return Created(string.Empty, new { Message = "InformationGroup added successfully", InformationGroup = infoGroup });
    }
}