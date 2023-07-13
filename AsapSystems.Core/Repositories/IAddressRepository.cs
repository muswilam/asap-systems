using AsapSystems.Core.Entities;

namespace AsapSystems.Core.Repositories
{
    public interface IAddressRepository : IRepository<Address>
    {
        Task<IEnumerable<AddressType>> GetAddressTypsAsync();
        
        Task<Address> GetAddress(int id);
        
        Task<IEnumerable<Address>> GetPagedAddressesByTypeAndSearchAsync(int personId, int pageSize, int pageNumber, string search, int? addressTypeId);
        Task<int> GetAddressesByTypeAndSearchCountAsync(int personId, string search, int? addressTypeId);
    }
}