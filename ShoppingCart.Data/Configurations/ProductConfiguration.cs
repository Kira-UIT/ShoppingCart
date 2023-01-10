using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingCart.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(d => d.Discount).WithMany(p => p.Products).HasForeignKey(x => x.DiscountId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(d => d.ProductCategory).WithMany(p => p.Products).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Cascade);
            builder.HasQueryFilter(u => !u.IsDeleted);
        }
    }
}
