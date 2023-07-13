using AsapSystems.BLL.Dtos.Addresses;
using AsapSystems.BLL.Services.Addresses;
using Microsoft.AspNetCore.Mvc;

namespace AsapSystems.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressController : BaseController
{
    private readonly IAddressService _addressService;

    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpGet("GetAddressTypesLookup")]
    public async Task<IActionResult> GetAddressTypesLookupAsync()
    {
        var result = await _addressService.GetAddressTypesLookupAsync();

        return Ok(result);
    }

    [HttpGet("GetAddresses")]
    public async Task<IActionResult> GetAddressesByFilterAsync([FromQuery] AddressFilterDto filterDto)
    {
        var result = await _addressService.GetAddressesByFilterAsync(filterDto);

        return Ok(result);
    }

    [HttpGet("GetAddress")]
    public async Task<IActionResult> GetAddressAsync(int id)
    {
        var result = await _addressService.GetAddressAsync(id);

        return Ok(result);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateAsync(NewAddressDto newAddressDto)
    {
        var result = await _addressService.CreateAddressAsync(newAddressDto, CurrentUserId);

        return Ok(result);
    }

    [HttpPost("Update")]
    public async Task<IActionResult> UpdateAsync(EditAddressDto editAddressDto)
    {
        var result = await _addressService.UpdateAddressAsync(editAddressDto, CurrentUserId);

        return Ok(result);
    }

    [HttpPost("Delete")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var result = await _addressService.DeleteAddressAsync(id);

        return Ok(result);
    }
}
