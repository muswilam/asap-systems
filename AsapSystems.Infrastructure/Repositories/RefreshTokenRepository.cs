using AsapSystems.Core.Entities;
using AsapSystems.Core.Repositories;
using AsapSystems.Infrastructure.Context;

namespace AsapSystems.Infrastructure.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly AsapContext _asapContext;

        public RefreshTokenRepository(AsapContext asapContext) 
            : base(asapContext)
        {
            _asapContext = _context as AsapContext;
        }
    }
}