using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Models
{
    public class Product : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public Guid DiscountId { get; set; }
        public Discount Discount { get; set; }
        public Guid CategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public Guid InvetoryId { get; set; }
        public ProductInventory ProductInventory { get; set; }
        public OrderItem OrderItem { get; set; }
    }
}
