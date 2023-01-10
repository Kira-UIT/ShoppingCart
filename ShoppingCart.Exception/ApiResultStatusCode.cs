using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ShoppingCart.Common
{
    public enum ApiResultStatusCode
    {
        [Display(Name = "Success")]
        Success = 0,

        [Display(Name = "Server error")]
        ServerError = 1,

        [Display(Name = "Bad request")]
        BadRequest = 2,

        [Display(Name = "Not found")]
        NotFound = 3,

        [Display(Name = "Empty list")]
        ListEmpty = 4,

        [Display(Name = "A processing error occurred")]
        LogicError = 5,

        [Display(Name = "Authentication error")]
        UnAuthorized = 6
    }
}
