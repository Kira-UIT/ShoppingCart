using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Models
{
    public class OrderDetail : BaseEntity<Guid>
    {
        public decimal Total { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid PaymentId { get; set; }
        public PaymentDetail PaymentDetail { get; set; }
        public ICollection<OrderItem> OrderItem { get; set; }
    }
}
