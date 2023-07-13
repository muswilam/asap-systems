using AsapSystems.BLL.Dtos.Addresses;
using AsapSystems.BLL.Dtos.Base;
using AsapSystems.BLL.Dtos.Lookups;
using AsapSystems.BLL.Helpers.ResponseHandler;

namespace AsapSystems.BLL.Services.Addresses
{
    public interface IAddressService 
    {
        Task<Response<IEnumerable<LookupDto>>> GetAddressTypesLookupAsync();
        
        Task<Response<Paged<IEnumerable<AddressDto>>>> GetAddressesByFilterAsync(AddressFilterDto filterDto);

        Task<Response<AddressDetailsDto>> GetAddressAsync(int addressId);

        Task<Response<int>> CreateAddressAsync(NewAddressDto newAddressDto, int currentUserId);

        Task<Response<bool>> UpdateAddressAsync(EditAddressDto editAddressDto, int currentUserId);

        Task<Response<bool>> DeleteAddressAsync(int addressId);
    }
}