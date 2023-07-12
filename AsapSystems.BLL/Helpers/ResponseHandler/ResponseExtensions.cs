namespace AsapSystems.BLL.Helpers.ResponseHandler
{
    public static class ResponseExtensions
    {
        public static Response<T> AddError<T>(this Response<T> response, string message, params string[] fields)
        {
            if(response.IsSuccess)
                response.IsSuccess = false;

            var error = new Error
            {
                Message = message
            };

            if (fields.Any())
            {
                error.OnField = true;
                error.Fields = fields.ToList();
            }

            response.Errors.Add(error);

            return response;
        }

        public static Response<T> AddErrors<T>(this Response<T> response, List<Error> errors)
        {
            if(response.IsSuccess)
                response.IsSuccess = false;

            response.Errors.AddRange(errors);

            return response;
        }
    }
}