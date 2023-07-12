using AsapSystems.BLL.Dtos.Auth;
using AsapSystems.BLL.Helpers.ResponseHandler;
using Microsoft.IdentityModel.Tokens;

namespace AsapSystems.BLL.Services.Auth
{
    public interface IAuthService
    {
        Task<Response<TokenResultDto>> RegisterAsync(RegisterDto registerDto);
        Task<Response<TokenResultDto>> LoginAsync(LoginDto loginDto);
        Task<Response<TokenResultDto>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto, TokenValidationParameters tokenValidationParameters);
        Task<Response<bool>> LogoutAsync(LogoutDto logoutDto);
    }
}