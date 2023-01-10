using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Models
{
    public class PaymentDetail : BaseEntity<Guid>
    {
        public int Amount { get; set; }
        public string Provider { get; set; }
        public string Status { get; set; }
        public Guid OrderId { get; set; }
        public OrderDetail OrderDetail { get; set; }
    }
}
