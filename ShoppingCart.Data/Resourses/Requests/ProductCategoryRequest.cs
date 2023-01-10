using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Resourses.Requests
{
    public class ProductCategoryRequest
    {
        [Required(ErrorMessage = "Product category name can not be null")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
