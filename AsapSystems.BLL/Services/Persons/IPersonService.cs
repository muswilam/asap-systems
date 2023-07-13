using AsapSystems.BLL.Dtos.Base;
using AsapSystems.BLL.Dtos.Persons;
using AsapSystems.BLL.Helpers.ResponseHandler;

namespace AsapSystems.BLL.Services.Persons
{
    public interface IPersonService 
    {
        Task<Response<Paged<IEnumerable<PersonDto>>>> GetPersonsByFilterAsync(PersonFilterDto filterDto);
    }
}