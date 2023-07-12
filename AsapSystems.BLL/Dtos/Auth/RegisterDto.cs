namespace AsapSystems.BLL.Dtos.Auth
{
    public record RegisterDto(string Name,
                              string Email,
                              string Password,
                              int GenderId);
}