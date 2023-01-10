using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Resourses.Responses
{
    public class CheckoutResponse
    {
        public Guid SessionId { get; set; }
        public string PubKey { get; set; }
    }
}
