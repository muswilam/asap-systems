using AsapSystems.Core.Entities;
using AsapSystems.Core.Repositories;
using AsapSystems.Infrastructure.Context;

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
    }
}