using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Google.Protobuf.Collections;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger): DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => request.ProductName == x.ProductName);

            if (coupon is null)
                coupon = new Models.Coupon { ProductName = "No Discount", Amount = 0, Description = "No Coupon" };

            logger.LogInformation("Discount is retrieved for ProductName : {productName}", request.ProductName);
            return coupon.Adapt<CouponModel>();
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            if (request.Coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request params"));
            
            // Check for existing discount
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.Coupon.ProductName);
            if (coupon is not null)
            {
                coupon.Amount = request.Coupon.Amount;
                coupon.Description = request.Coupon.Description;
            }
            else
            {
                coupon = request.Coupon.Adapt<Coupon>();
                dbContext.Coupons.Add(coupon);
            }
            
            await dbContext.SaveChangesAsync();

            return coupon.Adapt<CouponModel>();
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            if (request.Coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request params"));

            // Check for existing discount
            var coupon = request.Coupon.Adapt<Coupon>();
            dbContext.Coupons.Update(coupon);

            await dbContext.SaveChangesAsync();

            return coupon.Adapt<CouponModel>();
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons
                .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

            if (coupon is not null)
                dbContext.Coupons.Remove(coupon);
            await dbContext.SaveChangesAsync();
            return new DeleteDiscountResponse { Success = true };
        }

        public override async Task<CouponsModel> GetAllDiscounts(GetAllDiscountsRequest request, ServerCallContext context)
        {
            var discounts = await dbContext.Coupons.ToArrayAsync();

            var res = new CouponsModel();
            
            foreach (var discount in discounts)
            {
                res.Coupons.Add(discount.Adapt<CouponModel>());
            }

            return res;
        }
    }
}
