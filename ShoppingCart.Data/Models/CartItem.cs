using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Models
{
    public class CartItem : BaseEntity<Guid>
    {
        public int Quantity { get; set; }
        public Guid SessionId { get; set; }
        public ShoppingSession Sessions { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}

