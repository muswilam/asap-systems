using AsapSystems.BLL.Dtos.Lookups;
using AsapSystems.BLL.Helpers.ResponseHandler;

namespace AsapSystems.BLL.Services.Lookups
{
    public interface ILookupService
    {
        Task<Response<IEnumerable<LookupDto>>> GetGendersLookupAsync();
    }
}