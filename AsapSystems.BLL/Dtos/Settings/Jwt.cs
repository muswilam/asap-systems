namespace AsapSystems.BLL.Dtos.Settings
{
    public class Jwt
    {
        public string Secret { get; set; }

        public string Issuer { get; set; }

        public TimeSpan TokenExpiryTime { get; set; }

        public RefreshTokenSetting RefreshToken { get; set; } = new();
    }
}