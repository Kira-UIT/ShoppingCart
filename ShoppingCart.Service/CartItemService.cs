using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Base.Repositories;
using ShoppingCart.Base.Services;
using ShoppingCart.Data.Models;
using ShoppingCart.Data.Resourses.Requests;
using ShoppingCart.Data.Resourses.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace ShoppingCart.Service
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IShoppingSessionRepository _shoppingSessionRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CartItemService(ICartItemRepository cartItemRepository, IShoppingSessionRepository shoppingSessionRepository, IProductRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _cartItemRepository = cartItemRepository;
            _shoppingSessionRepository = shoppingSessionRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> AddCartItem(Guid userId, CartItemRequest cartItemRequest)
        {
            var response = new BaseResponse();
            var shoppingSession = await _shoppingSessionRepository.FindByCondition(x => x.UserId == userId).FirstOrDefaultAsync();
            var product = await _productRepository.GetByIdAsync(cartItemRequest.ProductId);
            var cartItem = await _cartItemRepository.FindByCondition(x => x.ProductId == cartItemRequest.ProductId).FirstOrDefaultAsync();

            // delete cart item when quantity equals zero
            if (cartItemRequest.Quantity == 0)
            {
                if (cartItem is not null)
                {
                    await _cartItemRepository.Delete(cartItem);
                }
            }
            else
            {
                // check cart item is existed
                if (cartItem is not null)
                {
                    cartItem.Quantity = cartItemRequest.Quantity;
                    _cartItemRepository.Update(cartItem);
                }
                else
                {
                    var newCartItem = _mapper.Map<CartItem>(cartItemRequest);
                    newCartItem.SessionId = shoppingSession.Id;
                    await _cartItemRepository.CreateAsync(newCartItem);
                }
            }
            await _unitOfWork.SaveChangesAsync();

            // get all list cart item of current user to calculate total
            var cartItemList = await _cartItemRepository.FindByCondition(x => x.SessionId == shoppingSession.Id).Include(x => x.Product).ThenInclude(x => x.Discount).ToListAsync();
            decimal total = 0;
            foreach (var cartItemChild in cartItemList)
            {
                if (cartItemChild.Product.Discount is not null)
                {
                    if (cartItemChild.Quantity > 1)
                    {
                        total += (cartItemChild.Quantity - 1) * cartItemChild.Product.Price + cartItemChild.Product.Price * cartItemChild.Product.Discount.DiscountPercent / 100;
                    }
                    else
                    {
                        total += cartItemChild.Quantity * cartItemChild.Product.Price * cartItemChild.Product.Discount.DiscountPercent / 100;
                    }
                }
                else
                {
                    total += cartItemChild.Quantity * cartItemChild.Product.Price;
                }
            }
            shoppingSession.Total = total;

            // update total in shopping session
            _shoppingSessionRepository.Update(shoppingSession);
            await _unitOfWork.SaveChangesAsync();
            return response;
        }

        public async Task<BaseResponse> DeleteCartItem(Guid userId, Guid productId)
        {
            var response = new BaseResponse();
            var shoppingSession = await _shoppingSessionRepository.FindByCondition(x => x.UserId == userId).FirstOrDefaultAsync();
            var product = await _productRepository.GetByIdAsync(productId);
            var cartItem = await _cartItemRepository.FindByCondition(x => x.ProductId == productId).FirstOrDefaultAsync();
            if (cartItem is not null)
            {
                await _cartItemRepository.Delete(cartItem);
                await _unitOfWork.SaveChangesAsync();
                // get all list cart item of current user to calculate total
                var cartItemList = await _cartItemRepository.FindByCondition(x => x.SessionId == shoppingSession.Id).Include(x => x.Product).ThenInclude(x => x.Discount).ToListAsync();
                decimal total = 0;
                foreach (var cartItemChild in cartItemList)
                {
                    if (cartItemChild.Product.Discount is not null)
                    {
                        if (cartItemChild.Quantity > 1)
                        {
                            total += (cartItemChild.Quantity - 1) * cartItemChild.Product.Price + cartItemChild.Product.Price * cartItemChild.Product.Discount.DiscountPercent / 100;
                        }
                        else
                        {
                            total += cartItemChild.Quantity * cartItemChild.Product.Price * cartItemChild.Product.Discount.DiscountPercent / 100;
                        }
                    }
                    else
                    {
                        total += cartItemChild.Quantity * cartItemChild.Product.Price;
                    }
                }
                shoppingSession.Total = total;

                // update total in shopping session
                _shoppingSessionRepository.Update(shoppingSession);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.BadRequest, "Cart item is not existed"));
            }
            return response;
        }
    }
}
