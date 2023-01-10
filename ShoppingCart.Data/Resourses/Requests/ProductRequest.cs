using ShoppingCart.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Resourses.Requests
{
    public class ProductRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public Guid DiscountId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid InventoryId { get; set; }
        public int Quantity { get; set; }
    }
}
