using AsapSystems.BLL.Dtos.Lookups;
using AsapSystems.BLL.Helpers.ResponseHandler;
using AsapSystems.Core;

namespace AsapSystems.BLL.Services.Lookups
{
    public class LookupService : ILookupService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LookupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<IEnumerable<LookupDto>>> GetGendersLookupAsync()
        {
            var response = new Response<IEnumerable<LookupDto>>();

            try
            {
                var genders = await _unitOfWork.GenderRepository.GetAllAsync();

                var gendersLookup = new List<LookupDto>();

                if (genders.Any())
                    gendersLookup.AddRange(genders.Select(g => new LookupDto
                    {
                        Id = g.Id,
                        Name = g.Name
                    }));

                response.Data = gendersLookup;
                
                return response;
            }
            catch (Exception ex)
            {
                response.AddError("Internal error.");
            }

            return response;
        }
    }
}