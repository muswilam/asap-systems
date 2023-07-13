using AsapSystems.Core.Entities;
using AsapSystems.Core.Repositories;
using AsapSystems.Infrastructure.Context;

namespace AsapSystems.Infrastructure.Repositories
{
    public class GenderRepository : Repository<Gender>, IGenderRepository
    {
        private readonly AsapContext _asapContext;

        public GenderRepository(AsapContext asapContext) 
            : base(asapContext)
        {
            _asapContext = _context as AsapContext;
        }
    }
}