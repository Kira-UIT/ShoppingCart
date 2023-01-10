
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Models
{
    public class ProductInventory : BaseEntity<Guid>
    {
        public int Quantity { get; set; }
        public Product Product { get; set; }
    }
}
