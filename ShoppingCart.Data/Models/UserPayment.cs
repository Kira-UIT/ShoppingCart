using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Models
{
    public class UserPayment
    {
        public Guid Id { get; set; }
        public string PaymentType { get; set; }
        public string Provider { get; set; }
        public int AccountNo { get; set; }
        public DateTime Expiry { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
