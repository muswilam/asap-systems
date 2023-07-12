namespace AsapSystems.BLL.Helpers.Security
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);

        bool VerifyHashedPassword(string providedPassword, string hashedPassword);
    }
}