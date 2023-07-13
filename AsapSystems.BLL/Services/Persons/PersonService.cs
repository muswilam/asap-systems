using AsapSystems.BLL.Dtos.Base;
using AsapSystems.BLL.Dtos.Persons;
using AsapSystems.BLL.Helpers.ResponseHandler;
using AsapSystems.Core;

namespace AsapSystems.BLL.Services.Persons
{
    public class PersonService : IPersonService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PersonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<Paged<IEnumerable<PersonDto>>>> GetPersonsByFilterAsync(PersonFilterDto filterDto)
        {
            var response = new Response<Paged<IEnumerable<PersonDto>>>();

            try
            {
                var filteredPersons = await _unitOfWork.PersonRepository.GetPagedPersonsByGenderAndSearchAsync(filterDto.PageSize,
                                                                                                               filterDto.PageNumber,
                                                                                                               filterDto.Search,
                                                                                                               filterDto.GenderId);
                                                                                                               
                var personsCount = await _unitOfWork.PersonRepository.GetPersonsByGenderAndSearchCountAsync(filterDto.Search, filterDto.GenderId);

                var personsDto = new List<PersonDto>();

                if (filteredPersons.Any())
                    personsDto.AddRange(filteredPersons.Select(fp => new PersonDto
                    {
                        Id = fp.Id,
                        Name = fp.Name,
                        Email = fp.Email,
                        GenderName = fp.Gender.Name,
                        JoinDate = fp.CreateDate
                    }));

                response.Data = new Paged<IEnumerable<PersonDto>>
                {
                    Items = personsDto,
                    TotalCount = personsCount
                };
            }
            catch (Exception ex)
            {
                response.AddError("Internal error.");
            }

            return response;
        }
    }
}