using AsapSystems.BLL.Services.Lookups;
using Microsoft.AspNetCore.Mvc;

namespace AsapSystems.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LookupController : ControllerBase
{
    private readonly ILookupService _lookupService;

    public LookupController(ILookupService lookupService)
    {
        _lookupService = lookupService;
    }

    [HttpGet("GetGendersLookup")]
    public async Task<IActionResult> GetGendersLookupAsync()
    {
        var result = await _lookupService.GetGendersLookupAsync();

        return Ok(result);
    }
}
