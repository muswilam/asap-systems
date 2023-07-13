using AsapSystems.BLL.Dtos.Persons;
using AsapSystems.BLL.Services.Persons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsapSystems.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : BaseController
{
    private readonly IPersonService _personService;

    public PersonController(IPersonService personService)
    {
        _personService = personService;
    }

    [HttpGet("GetPersons")]
    public async Task<IActionResult> GetPersonsByFilterAsync([FromQuery]PersonFilterDto filterDto)
    {
        var result = await _personService.GetPersonsByFilterAsync(filterDto);

        return Ok(result);
    }
}
