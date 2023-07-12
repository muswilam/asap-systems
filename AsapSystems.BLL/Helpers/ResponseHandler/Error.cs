namespace AsapSystems.BLL.Helpers.ResponseHandler
{
    public class Error
    {
        public bool OnField { get; set; } = false;

        public string Message { get; set; } = string.Empty;

        public List<string> Fields { get; set; } = new();
    }
}