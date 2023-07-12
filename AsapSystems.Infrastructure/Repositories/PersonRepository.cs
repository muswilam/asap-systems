using AsapSystems.Core.Entities;
using AsapSystems.Core.Repositories;
using AsapSystems.Infrastructure.Context;

namespace AsapSystems.Infrastructure.Repositories
{
    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        private readonly AsapContext _asapContext;

        public PersonRepository(AsapContext asapContext) 
            : base(asapContext)
        {
            _asapContext = _context as AsapContext;
        }
    }
}