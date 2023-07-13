using AsapSystems.Core;
using AsapSystems.Core.Repositories;
using AsapSystems.Infrastructure.Context;

namespace AsapSystems.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AsapContext _context;

        public UnitOfWork(AsapContext context,
                          IPersonRepository personRepository,
                          IAddressRepository addressRepository,
                          IRefreshTokenRepository refreshTokenRepository,
                          IGenderRepository genderRepository)
        {
            _context = context;
            PersonRepository = personRepository;
            AddressRepository = addressRepository;
            RefreshTokenRepository = refreshTokenRepository;
            GenderRepository = genderRepository;
        }

        public IPersonRepository PersonRepository { get; }
        public IAddressRepository AddressRepository { get; }
        public IRefreshTokenRepository RefreshTokenRepository { get; }
        public IGenderRepository GenderRepository { get; }

        public void Commit() =>
            _context.SaveChanges();

        public async Task CommitAsync() =>
            await _context.SaveChangesAsync();

        public void Rollback() =>
            _context.Dispose();

        public async Task RollbackAsync() =>
            await _context.DisposeAsync();
    }
}