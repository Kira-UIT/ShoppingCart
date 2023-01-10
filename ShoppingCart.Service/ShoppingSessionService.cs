using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Base.Repositories;
using ShoppingCart.Base.Services;
using ShoppingCart.Data.Resourses.Responses;
using ShoppingCart.Repository;
using Stripe.BillingPortal;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stripe;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Service
{
    public class ShoppingSessionService : IShoppingSessionService
    {
        private readonly IShoppingSessionRepository _shoppingSessionRepository;
        private readonly IMapper _mapper;

        public ShoppingSessionService(IShoppingSessionRepository shoppingSessionRepository, IMapper mapper)
        {
            _shoppingSessionRepository = shoppingSessionRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<ShoppingSessionResponse>> GetShoppingSession(Guid userId)
        {
            var response = new BaseResponse<ShoppingSessionResponse>();
            var shoppingSession = await _shoppingSessionRepository.FindByCondition(x => x.UserId == userId, false).Include(x => x.CartItem).FirstOrDefaultAsync();
            if (shoppingSession is not null)
            {
                response.Data = _mapper.Map<ShoppingSessionResponse>(shoppingSession);
            }
            return response;
        }
    }
}
