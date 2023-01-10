using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using ShoppingCart.Data.Models;
using ShoppingCart.Data.Resourses.Requests;
using ShoppingCart.Data.Resourses.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data
{
    public class MapperService : Profile
    {
        public MapperService()
        {
            CreateMap<DiscountResponse, Discount>().ReverseMap();
            CreateMap<DiscountRequest, Discount>().ReverseMap();
            CreateMap<JsonPatchDocument<DiscountRequest>, JsonPatchDocument<Discount>>().ReverseMap();

            CreateMap<RegisterRequest, User>().ReverseMap();
            CreateMap<GetCurrentUserResponse, User>().ReverseMap();
            CreateMap<ChangePasswordRequest, User>().ReverseMap();

            CreateMap<ProductCategoryRequest, ProductCategory>().ReverseMap();
            CreateMap<ProductCategoryResponse, ProductCategory>().ReverseMap();

            CreateMap<ProductInventoryResponse, ProductInventory>().ReverseMap();

            CreateMap<ProductRequest, Product>().ReverseMap();
            CreateMap<ProductResponse, Product>().ReverseMap();

            CreateMap<ShoppingSessionResponse, ShoppingSession>().ReverseMap();

            CreateMap<CartItemRequest, CartItem>().ReverseMap();
            CreateMap<CartItemResponse, CartItem>().ReverseMap();

            CreateMap<GetCurrentUserResponse, User>().ReverseMap();
        }
    }
}
