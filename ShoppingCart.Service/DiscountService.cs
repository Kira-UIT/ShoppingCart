using ShoppingCart.Base.Services;
using ShoppingCart.Data.Resourses.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Repository;
using ShoppingCart.Data;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using ShoppingCart.Base.Repositories;
using ShoppingCart.Data.Resourses.Requests;
using System.Net;
using ShoppingCart.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;

namespace ShoppingCart.Service
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public DiscountService(IDiscountRepository discountRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _discountRepository = discountRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> CreateDiscount(DiscountRequest discountRequest)
        {
            var response = new BaseResponse();
            if (discountRequest is null)
            {
                response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.NoContent, "No content"));
            }
            else
            {
                await _discountRepository.CreateAsync(_mapper.Map<Discount>(discountRequest));
                await _unitOfWork.SaveChangesAsync();
            }
            return response;
        }

        public async Task<BaseResponse> DeleteDiscount(Guid Id)
        {
            var response = new BaseResponse();
            var result = await _discountRepository.GetByIdAsync(Id);
            if (result is null)
            {
                response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.NotFound, "Discount is not existed"));
            }
            else
            {
                if (result.IsDeleted)
                {
                    response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.BadRequest, "This discount has been deleted"));
                }
                else
                {
                    _discountRepository.Remove(result);
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            return response;
        }

        public async Task<BaseResponse<List<DiscountResponse>>> GetDiscount()
        {
            var response = new BaseResponse<List<DiscountResponse>>();
            var result = await _discountRepository.BuildQuery().ToListAsync(c => c);
            response.Data = _mapper.Map<List<DiscountResponse>>(result);
            return response;
        }

        public async Task<BaseResponse<DiscountResponse>> GetDiscountById(Guid Id)
        {
            var response = new BaseResponse<DiscountResponse>();
            var discount = await _discountRepository.GetByIdAsync(Id);
            if (discount is null)
            {
                response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.NotFound, "Discount is not existed"));
            }
            else
            {
                response.Data = _mapper.Map<DiscountResponse>(discount);
            }
            return response;
        }

        public async Task<BaseResponse> UpdateDiscount(Guid Id, DiscountRequest discountRequest)
        {
            var response = new BaseResponse();
            var result = await _discountRepository.FindByCondition(x => x.Id == Id, false).FirstOrDefaultAsync();
            if (result is null)
            {
                response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.NotFound, "Discount is not found"));
            }
            else
            {
                result.ModifiedAt = DateTime.UtcNow;
                result.DiscountPercent = discountRequest.DiscountPercent;
                result.Description = discountRequest.Description;
                result.Name = discountRequest.Name;
                _discountRepository.Update(result);
                await _unitOfWork.SaveChangesAsync();
            }
            return response;
        }

        // Not done yet
        public async Task<BaseResponse> UpdateDiscountPatch(Guid Id, JsonPatchDocument<DiscountRequest> jsonPatchDocument)
        {
            var response = new BaseResponse();
            var discount = await _discountRepository.FindByCondition(x => x.Id == Id, false).FirstOrDefaultAsync();
            if (discount is null)
            {
                response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.NotFound, "Discount is not existed"));
            }
            else
            {
                var patch = _mapper.Map<JsonPatchDocument<Discount>>(jsonPatchDocument);
                patch.ApplyTo(discount);
                _discountRepository.Update(discount);
                await _unitOfWork.SaveChangesAsync();
            }
            return response;
        }
    }
}
