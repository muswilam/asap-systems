using AsapSystems.BLL.Dtos.Auth;
using AsapSystems.BLL.Helpers.Security;
using AsapSystems.BLL.Helpers.ResponseHandler;
using AsapSystems.Core;
using AsapSystems.Core.Entities;
using AsapSystems.Core.Enums;

namespace AsapSystems.BLL.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<Response<bool>> RegisterAsync(RegisterDto registerDto)
        {
            var response = new Response<bool>();

            try
            {
                // validation.
                var validationResult = await IsRegisterValidAsync(registerDto);

                if(!validationResult.IsSuccess)
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
                await _unitOfWork.CommitAsync();
                
                // create jwt token.

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

            if(string.IsNullOrEmpty(registerDto.Name))
                response.AddError("Name is required", nameof(registerDto.Name));

            if(string.IsNullOrEmpty(registerDto.Email))
                response.AddError("Email is required", nameof(registerDto.Email));

            if(string.IsNullOrEmpty(registerDto.Password))
                response.AddError("Password is required", nameof(registerDto.Password));

            if(!Enum.IsDefined(typeof(GenderEnum), registerDto.GenderId))
                response.AddError("Invalid gender.", nameof(registerDto.GenderId));

            if(await _unitOfWork.PersonRepository.AnyAsync(p => p.Email.ToLower().Equals(registerDto.Email.ToLower())))
                response.AddError("Email is already exist.", nameof(registerDto.Email));

            return response;
        }
        #endregion
    }
}