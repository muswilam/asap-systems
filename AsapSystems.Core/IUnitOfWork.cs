using AsapSystems.Core.Repositories;

namespace AsapSystems.Core
{
    public interface IUnitOfWork
    {
        IPersonRepository PersonRepository { get; }
        IAddressRepository AddressRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }

        void Commit();
        Task CommitAsync();

        void Rollback();
        Task RollbackAsync();
    }
}