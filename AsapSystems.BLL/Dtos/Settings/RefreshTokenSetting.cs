namespace AsapSystems.BLL.Dtos.Settings
{
    public class RefreshTokenSetting
    {
        public int TokenLength { get; set; }

        public int RefreshTokenExpiryInMonths { get; set; }

        public string Chars { get; set; }
    }
}