using AsapSystems.Core.Entities;
using AsapSystems.Core.Repositories;
using AsapSystems.Infrastructure.Context;
using AsapSystems.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<Person>> GetPagedPersonsByGenderAndSearchAsync(int pageSize, int pageNumber, string search, int? genderId) =>
            await GetPersonsByGenderAndSearch(search, genderId)
                    .Skip(pageNumber * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

        public async Task<int> GetPersonsByGenderAndSearchCountAsync(string search, int? genderId) =>
            await GetPersonsByGenderAndSearch(search, genderId)
                    .CountAsync();


        private IQueryable<Person> GetPersonsByGenderAndSearch(string search, int? genderId) =>
             _asapContext.Persons
                .Include(p => p.Gender)
                .WhereIf(p => p.Name.ToLower().Contains(search.ToLower()), !string.IsNullOrEmpty(search))
                .WhereIf(p => p.GenderId == genderId, genderId.HasValue);
    }
}