using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Resourses.Responses
{
    public class BaseResponse<TResponse>
    {
        public TResponse Data { get; set; }
        public List<ErrorResponse> Errors { get; set; } = new List<ErrorResponse>();
        public bool IsSucceeded => !Errors.Any();
    }
    public class BaseResponse
    {
        public List<ErrorResponse> Errors { get; set; } = new List<ErrorResponse>();
        public bool IsSucceeded => !Errors.Any();
    }
}
