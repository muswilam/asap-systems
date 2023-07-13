using AsapSystems.BLL.Dtos.Auth;
using AsapSystems.BLL.Helpers.Security;
using AsapSystems.BLL.Helpers.ResponseHandler;
using AsapSystems.Core;
using AsapSystems.Core.Entities;
using AsapSystems.Core.Enums;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using AsapSystems.BLL.Dtos.Settings;
using Microsoft.Extensions.Options;

namespace AsapSystems.BLL.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly AuthSetting _authSetting;

        public AuthService(IUnitOfWork unitOfWork,
                           IPasswordHasher passwordHasher,
                           IOptions<AuthSetting> authSettingOptions)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _authSetting = authSettingOptions.Value;
        }

        public async Task<Response<TokenResultDto>> RegisterAsync(RegisterDto registerDto)
        {
            var response = new Response<TokenResultDto>();

            try
            {
                // validation.
                var validationResult = await IsRegisterValidAsync(registerDto);

                if (!validationResult.IsSuccess)
                    return response.AddErrors(validationResult.Errors);

                // hash password.
                var hashedPassword = _passwordHasher.HashPassword(registerDto.Password);

                // create person.
                var person = new Person
                {
                    Name = registerDto.Name,
                    Email = registerDto.Email,
                    Password = hashedPassword,
                    GenderId = registerDto.GenderId
                };

                await _unitOfWork.PersonRepository.AddAsync(person);

                // create jwt token.
                var generatedJwtToken = await GenerateJwtTokenAsync(person);

                var generatedRefreshToken = await GenerateRefreshTokenAsync(generatedJwtToken.Jti, person);

                await _unitOfWork.CommitAsync();

                response.Data = new TokenResultDto
                {
                    Token = generatedJwtToken.Token,
                    RefreshToken = generatedRefreshToken
                };

                return response;
            }
            catch (Exception ex)
            {
                return response.AddError("Internal error.");
            }
        }

        public async Task<Response<TokenResultDto>> LoginAsync(LoginDto loginDto)
        {
            var response = new Response<TokenResultDto>();

            try
            {
                var validation = IsLoginValid(loginDto);

                if (!validation.IsSuccess)
                    return response.AddErrors(validation.Errors);

                var person = await _unitOfWork.PersonRepository.SingleOrDefaultAsync(p => p.Email.ToLower().Equals(loginDto.Email.ToLower()));

                if (person is null)
                    return response.AddError("Invalid credentials.");

                var isPasswordVerified = _passwordHasher.VerifyHashedPassword(loginDto.Password, person.Password);

                if (!isPasswordVerified)
                    return response.AddError("Invalid credentials.");

                var generatedJwtToken = await GenerateJwtTokenAsync(person);

                var generatedRefreshToken = await GenerateRefreshTokenAsync(generatedJwtToken.Jti, person);

                await _unitOfWork.CommitAsync();

                response.Data = new TokenResultDto
                {
                    Token = generatedJwtToken.Token,
                    RefreshToken = generatedRefreshToken
                };

                return response;
            }
            catch (Exception ex)
            {
                return response.AddError("Internal error.");
            }
        }

        public async Task<Response<TokenResultDto>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto, TokenValidationParameters tokenValidationParameters)
        {
            var response = new Response<TokenResultDto>();

            try
            {
                var validation = IsRefreshTokenValid(refreshTokenDto);

                if (!validation.IsSuccess)
                    return response.AddErrors(validation.Errors);

                var verifyTokenResult = await VerifyTokenAsync(refreshTokenDto, tokenValidationParameters);

                if (!verifyTokenResult.IsSuccess)
                    return response.AddErrors(verifyTokenResult.Errors);

                var storedToken = verifyTokenResult.Data;

                // generate new tokens.
                var person = await _unitOfWork.PersonRepository.SingleOrDefaultAsync(e => e.Id == storedToken.PersonId);

                if (person is null)
                    return response.AddError("Person is not found", nameof(person));

                var newJwtToken = await GenerateJwtTokenAsync(person);

                var newRefreshToken = await UpdateRefreshTokenAsync(storedToken, newJwtToken.Jti);

                response.Data = new TokenResultDto
                {
                    Token = newJwtToken.Token,
                    RefreshToken = newRefreshToken
                };

                return response;
            }
            catch (Exception ex)
            {
                return response.AddError("Internal error.");
            }
        }

        public async Task<Response<bool>> LogoutAsync(LogoutDto logoutDto)
        {
            var response = new Response<bool>();

            try
            {
                var validation = IsLogoutValid(logoutDto);

                if (!validation.IsSuccess)
                    return response.AddErrors(validation.Errors);

                var existRefreshToken = await _unitOfWork.RefreshTokenRepository.SingleOrDefaultAsync(rt => rt.Token == logoutDto.RefreshToken);

                if (existRefreshToken is null)
                {
                    response.AddError("Refresh token is not found.", nameof(logoutDto.RefreshToken));
                    return response;
                }

                _unitOfWork.RefreshTokenRepository.Remove(existRefreshToken);
                await _unitOfWork.CommitAsync();

                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                return response.AddError("Internal error.");
            }
        }

        #region Validations.
        public async Task<Response<bool>> IsRegisterValidAsync(RegisterDto registerDto)
        {
            var response = new Response<bool>();

            if (string.IsNullOrEmpty(registerDto.Name))
                response.AddError("Name is required", nameof(registerDto.Name));

            if (string.IsNullOrEmpty(registerDto.Email))
                response.AddError("Email is required", nameof(registerDto.Email));

            if (string.IsNullOrEmpty(registerDto.Password))
                response.AddError("Password is required", nameof(registerDto.Password));

            if (!Enum.IsDefined(typeof(GenderEnum), registerDto.GenderId))
                response.AddError("Invalid gender.", nameof(registerDto.GenderId));

            if (await _unitOfWork.PersonRepository.AnyAsync(p => p.Email.ToLower().Equals(registerDto.Email.ToLower())))
                response.AddError("Email is already exist.", nameof(registerDto.Email));

            response.Data = true;
            return response;
        }

        public Response<bool> IsLoginValid(LoginDto loginDto)
        {
            var response = new Response<bool>();

            if (string.IsNullOrEmpty(loginDto.Email))
                response.AddError("Email is required", nameof(loginDto.Email));

            if (string.IsNullOrEmpty(loginDto.Password))
                response.AddError("Password is required", nameof(loginDto.Password));

            response.Data = true;
            return response;
        }

        public Response<bool> IsRefreshTokenValid(RefreshTokenDto refreshTokenDto)
        {
            var response = new Response<bool>();

            if (string.IsNullOrEmpty(refreshTokenDto.Token))
                response.AddError("Token is required", nameof(refreshTokenDto.Token));

            if (string.IsNullOrEmpty(refreshTokenDto.RefreshToken))
                response.AddError("Refresh token is required", nameof(refreshTokenDto.RefreshToken));

            response.Data = true;
            return response;
        }

        public Response<bool> IsLogoutValid(LogoutDto logoutDto)
        {
            var response = new Response<bool>();

            if (string.IsNullOrEmpty(logoutDto.RefreshToken))
                response.AddError("Refresh token is required", nameof(logoutDto.RefreshToken));

            response.Data = true;
            return response;
        }
        #endregion

        #region Jwt Token Helpers.
        private async Task<JwtTokenDto> GenerateJwtTokenAsync(Person person)
        {
            return await Task.Run(() =>
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.ASCII.GetBytes(_authSetting.Jwt.Secret);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = _authSetting.Jwt.Issuer,
                    Subject = new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(TokenClaimTypeEnum.Id.ToString(), person.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, person.Email),
                        new Claim(JwtRegisteredClaimNames.Sub, person.Email),
                        new Claim(TokenClaimTypeEnum.Name.ToString(), person.Name),

                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    }),
                    Expires = DateTime.UtcNow.Add(_authSetting.Jwt.TokenExpiryTime),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = jwtTokenHandler.CreateToken(tokenDescriptor);

                var jwtToken = jwtTokenHandler.WriteToken(token);

                return new JwtTokenDto
                {
                    Jti = token.Id,
                    Token = jwtToken,
                };
            });
        }

        private async Task<string> GenerateRefreshTokenAsync(string jti, Person person)
        {
            var refreshToken = new RefreshToken
            {
                Jti = jti,
                Person = person,
                ExpireDate = DateTime.UtcNow.AddMonths(_authSetting.Jwt.RefreshToken.RefreshTokenExpiryInMonths),
                CreateDate = DateTime.UtcNow,
                Token = $"{GenerateRandom(_authSetting.Jwt.RefreshToken.TokenLength)}{Guid.NewGuid()}"
            };

            await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken);

            return refreshToken.Token;
        }

        private async Task<string> UpdateRefreshTokenAsync(RefreshToken refreshToken, string jti)
        {
            refreshToken.Jti = jti;
            refreshToken.Token = $"{GenerateRandom(_authSetting.Jwt.RefreshToken.TokenLength)}{Guid.NewGuid()}";
            refreshToken.ModifiedDate = DateTime.UtcNow;

            _unitOfWork.RefreshTokenRepository.Update(refreshToken);
            await _unitOfWork.CommitAsync();

            return refreshToken.Token;
        }

        private async Task<Response<RefreshToken>> VerifyTokenAsync(RefreshTokenDto refreshTokenDto,
                                                                    TokenValidationParameters tokenValidationParameters)
        {
            var response = new Response<RefreshToken>();

            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();

                // prevent validate token lifetime.
                tokenValidationParameters.ValidateLifetime = false;

                // 01- Validate token is a propper jwt token formatting.
                var claimsPrincipal = jwtTokenHandler.ValidateToken(refreshTokenDto.Token, tokenValidationParameters, out var validatedToken);

                // 02- Validate token has been encrypted using the encryption that we've specified. 
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var isSameEncryption = jwtSecurityToken.Header
                                                           .Alg
                                                           .Equals(SecurityAlgorithms.HmacSha256,
                                                                   StringComparison.InvariantCultureIgnoreCase);

                    if (!isSameEncryption)
                    {
                        response.AddError("Invalid token.");

                        // reset lifetime to valdiate it.
                        tokenValidationParameters.ValidateLifetime = true;
                        return response;
                    }
                }

                // Todo-Sully: check validated token is not jwtSecurityToken.

                // 03- Validate token expiry date.
                var utcLongExpiryDate = long.Parse(claimsPrincipal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp).Value);

                var expiryDate = UnixTimeStampToDateTime(utcLongExpiryDate);

                if (expiryDate > DateTime.UtcNow)
                {
                    response.AddError("Token not expired yet.");

                    // reset lifetime to valdiate it.
                    tokenValidationParameters.ValidateLifetime = true;
                    return response;
                }

                // 04- Validate actual token stored in database.
                var storedToken = await _unitOfWork.RefreshTokenRepository.SingleOrDefaultAsync(rt => rt.Token == refreshTokenDto.RefreshToken);

                if (storedToken is null)
                {
                    response.AddError("Token not found.", nameof(refreshTokenDto.RefreshToken));

                    // reset lifetime to valdiate it.
                    tokenValidationParameters.ValidateLifetime = true;
                    return response;
                }

                // 07- Validate jwt token Jti matches refresh token jti in database.
                var jti = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

                if (!storedToken.Jti.Equals(jti))
                {
                    response.AddError("Mismatch tokens.");

                    // reset lifetime to valdiate it.
                    tokenValidationParameters.ValidateLifetime = true;
                    return response;
                }

                response.Data = storedToken;
            }
            catch (Exception ex)
            {
                response.AddError("Invalid token.");
            }

            // reset lifetime to valdiate it.
            tokenValidationParameters.ValidateLifetime = true;
            return response;
        }

        private string GenerateRandom(int length)
        {
            var random = new Random();

            return new string(Enumerable.Repeat(_authSetting.Jwt.RefreshToken.Chars, length)
                                        .Select(s => s[random.Next(s.Length)])
                                        .ToArray());
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // utc time is an integer (long) number of seconds from the 1970/1/1 till now. 
            var datetimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return datetimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();
        }
        #endregion
    }
}