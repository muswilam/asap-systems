using AsapSystems.Core.Entities;
using AsapSystems.Core.Repositories;
using AsapSystems.Infrastructure.Context;
using AsapSystems.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AsapSystems.Infrastructure.Repositories
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        private readonly AsapContext _asapContext;

        public AddressRepository(AsapContext asapContext)
            : base(asapContext)
        {
            _asapContext = _context as AsapContext;
        }

        public async Task<IEnumerable<AddressType>> GetAddressTypsAsync() =>
            await _asapContext.AddressTypes.ToListAsync();

        public async Task<Address> GetAddress(int id) =>
            await _asapContext.Addresses
                .Include(a => a.AddressType)
                .SingleOrDefaultAsync(a => a.Id == id);

        public async Task<IEnumerable<Address>> GetPagedAddressesByTypeAndSearchAsync(int personId, int pageSize, int pageNumber, string search, int? addressTypeId) =>
            await GetAddressesByTypeAndSearch(personId, search, addressTypeId)
                    .Skip(pageNumber * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

        public async Task<int> GetAddressesByTypeAndSearchCountAsync(int personId, string search, int? addressTypeId) =>
            await GetAddressesByTypeAndSearch(personId, search, addressTypeId)
                    .CountAsync();

        private IQueryable<Address> GetAddressesByTypeAndSearch(int personId, string search, int? addressTypeId) =>
             _asapContext.Addresses
                .Include(a => a.AddressType)
                .Where(a => a.PersonId == personId)
                .WhereIf(a => a.Name.ToLower().Contains(search.ToLower()), !string.IsNullOrEmpty(search))
                .WhereIf(a => a.AddressTypeId == addressTypeId, addressTypeId.HasValue);
    }
}