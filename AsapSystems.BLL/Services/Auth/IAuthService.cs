using AsapSystems.BLL.Dtos.Auth;
using AsapSystems.BLL.Helpers.ResponseHandler;

namespace AsapSystems.BLL.Services.Auth
{
    public interface IAuthService
    {
        Task<Response<bool>> RegisterAsync(RegisterDto registerDto);
    }
}