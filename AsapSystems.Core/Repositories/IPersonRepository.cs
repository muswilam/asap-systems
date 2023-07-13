using AsapSystems.Core.Entities;

namespace AsapSystems.Core.Repositories
{
    public interface IPersonRepository : IRepository<Person>
    {
        Task<IEnumerable<Person>> GetPagedPersonsByGenderAndSearchAsync(int pageSize, int pageNumber, string search, int? genderId);

        Task<int> GetPersonsByGenderAndSearchCountAsync(string search, int? genderId);
    }
}