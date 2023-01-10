using System.Net;

namespace ShoppingCart.Data.Resourses.Responses
{
    public class ErrorResponse
    {
        public HttpStatusCode Code { get; set; }
        public string Message { get; set; }
        public static ErrorResponse FromResource(HttpStatusCode code, string message)
        {
            return new ErrorResponse()
            {
                Code = code,
                Message = message,
            };
        }
    }
}
