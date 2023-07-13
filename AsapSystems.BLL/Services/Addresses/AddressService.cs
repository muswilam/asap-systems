using AsapSystems.BLL.Dtos.Addresses;
using AsapSystems.BLL.Dtos.Base;
using AsapSystems.BLL.Dtos.Lookups;
using AsapSystems.BLL.Helpers.ResponseHandler;
using AsapSystems.Core;
using AsapSystems.Core.Entities;
using AsapSystems.Core.Enums;

namespace AsapSystems.BLL.Services.Addresses
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<IEnumerable<LookupDto>>> GetAddressTypesLookupAsync()
        {
            var response = new Response<IEnumerable<LookupDto>>();

            var types = await _unitOfWork.AddressRepository.GetAddressTypsAsync();

            var typesDto = new List<LookupDto>();

            if (types.Any())
                typesDto.AddRange(types.Select(t => new LookupDto
                {
                    Id = t.Id,
                    Name = t.Name
                }));

            response.Data = typesDto;

            return response;
        }

        public async Task<Response<Paged<IEnumerable<AddressDto>>>> GetAddressesByFilterAsync(AddressFilterDto filterDto)
        {
            var response = new Response<Paged<IEnumerable<AddressDto>>>();

            try
            {
                if(filterDto.PersonId <= default(int))
                    return response.AddError("PersonId is required.");

                var filteredAddresses = await _unitOfWork.AddressRepository.GetPagedAddressesByTypeAndSearchAsync(filterDto.PersonId,
                                                                                                                  filterDto.PageSize,
                                                                                                                  filterDto.PageNumber,
                                                                                                                  filterDto.Search,
                                                                                                                  filterDto.AddressTypeId);

                var addressesCount = await _unitOfWork.AddressRepository.GetAddressesByTypeAndSearchCountAsync(filterDto.PersonId,
                                                                                                               filterDto.Search,
                                                                                                               filterDto.AddressTypeId);

                var personsDto = new List<AddressDto>();

                if (filteredAddresses.Any())
                    personsDto.AddRange(filteredAddresses.Select(fp => new AddressDto
                    {
                        Id = fp.Id,
                        Name = fp.Name,
                        AddressTypeName = fp.AddressType.Name
                    }));

                response.Data = new Paged<IEnumerable<AddressDto>>
                {
                    Items = personsDto,
                    TotalCount = addressesCount
                };
            }
            catch (Exception ex)
            {
                response.AddError("Internal error.");
            }

            return response;
        }

        public async Task<Response<AddressDetailsDto>> GetAddressAsync(int addressId)
        {
            var response = new Response<AddressDetailsDto>();

            try
            {
                var address = await _unitOfWork.AddressRepository.GetAddress(addressId);

                if (address is null)
                    return response.AddError("Address is not found.");

                response.Data = new AddressDetailsDto
                {
                    Id = address.Id,
                    Name = address.Name,
                    Country = address.Country,
                    City = address.City,
                    Street = address.Street,
                    BuildingNumber = address.BuildingNumber,
                    ApartmentNumber = address.ApartmentNumber,
                    AddressTypeName = address.AddressType.Name,
                };
            }
            catch (Exception ex)
            {
                response.AddError("Internal error.");
            }

            return response;
        }

        public async Task<Response<int>> CreateAddressAsync(NewAddressDto newAddressDto, int currentUserId)
        {
            var response = new Response<int>();

            try
            {
                // validation.
                var validation = await IsAddressValid(newAddressDto);

                if (!validation.IsSuccess)
                    return response.AddErrors(validation.Errors);

                // mapping.
                var newAddress = new Address
                {
                    PersonId = newAddressDto.PersonId,
                    Name = newAddressDto.Name,
                    Country = newAddressDto.Country,
                    City = newAddressDto.City,
                    BuildingNumber = newAddressDto.BuildingNumber,
                    ApartmentNumber = newAddressDto.ApartmentNumber,
                    AddressTypeId = newAddressDto.AddressTypeId,
                    CreateDate = DateTime.UtcNow,
                    CreateBy = currentUserId
                };

                await _unitOfWork.AddressRepository.AddAsync(newAddress);
                await _unitOfWork.CommitAsync();

                response.Data = newAddress.Id;
            }
            catch (Exception ex)
            {
                response.AddError("Internal error.");
            }

            return response;
        }

        public async Task<Response<bool>> UpdateAddressAsync(EditAddressDto editAddressDto, int currentUserId)
        {
            var response = new Response<bool>();

            try
            {
                // validation.
                var validation = await IsAddressValid(editAddressDto, editAddressDto.Id);

                if (!validation.IsSuccess)
                    return response.AddErrors(validation.Errors);

                var address = await _unitOfWork.AddressRepository.GetAsync(editAddressDto.Id);

                if (address is null)
                    return response.AddError("Address is not found.");

                // mapping.
                address.Name = editAddressDto.Name;
                address.Country = editAddressDto.Country;
                address.City = editAddressDto.City;
                address.BuildingNumber = editAddressDto.BuildingNumber;
                address.ApartmentNumber = editAddressDto.ApartmentNumber;
                address.AddressTypeId = editAddressDto.AddressTypeId;
                address.ModifiedDate = DateTime.UtcNow;
                address.ModifiedBy = currentUserId;

                _unitOfWork.AddressRepository.Update(address);
                await _unitOfWork.CommitAsync();

                response.Data = true;
            }
            catch (Exception ex)
            {
                response.AddError("Internal error.");
            }

            return response;
        }

        public async Task<Response<bool>> DeleteAddressAsync(int addressId)
        {
            var response = new Response<bool>();

            try
            {
                var address = await _unitOfWork.AddressRepository.GetAsync(addressId);

                if (address is null)
                    return response.AddError("Address is not found.");

                _unitOfWork.AddressRepository.Remove(address);
                await _unitOfWork.CommitAsync();

                response.Data = true;

                return response;
            }
            catch (Exception ex)
            {
                response.AddError("Internal error.");
            }

            return response;
        }

        #region Validation.
        private async Task<Response<bool>> IsAddressValid(NewAddressDto newAddressDto, int? addressId = null)
        {
            var response = new Response<bool>();

            if (!await _unitOfWork.PersonRepository.AnyAsync(p => p.Id == newAddressDto.PersonId))
                response.AddError("Invalid personId", nameof(newAddressDto.PersonId));

            if (string.IsNullOrEmpty(newAddressDto.Name))
                response.AddError("Name is required.", nameof(newAddressDto.Name));

            if (string.IsNullOrEmpty(newAddressDto.Country))
                response.AddError("Country is required.", nameof(newAddressDto.Country));

            if (string.IsNullOrEmpty(newAddressDto.City))
                response.AddError("City is required.", nameof(newAddressDto.City));

            if (string.IsNullOrEmpty(newAddressDto.Street))
                response.AddError("Street is required.", nameof(newAddressDto.Street));

            if (!Enum.IsDefined(typeof(AddressTypeEnum), newAddressDto.AddressTypeId))
                response.AddError("Invalid address type..", nameof(newAddressDto.AddressTypeId));

            if (await _unitOfWork.AddressRepository.AnyAsync(a => a.Name.ToLower().Equals(newAddressDto.Name.ToLower()) && a.PersonId == newAddressDto.PersonId && a.Id != addressId))
                response.AddError("Name is already exist.", nameof(newAddressDto.Name));

            response.Data = true;
            return response;
        }
        #endregion
    }
}