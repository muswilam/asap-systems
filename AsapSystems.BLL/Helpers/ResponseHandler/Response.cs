namespace AsapSystems.BLL.Helpers.ResponseHandler
{
    public class Response<T>
    {
        public bool IsSuccess { get; set; } = true;

        public List<Error> Errors { get; set; } = new();

        public T Data { get; set; }
    }
}