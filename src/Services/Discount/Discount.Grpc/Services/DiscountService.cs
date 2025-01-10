using Discount.Grpc.Data;
using Discount.Grpc.Models;
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

        public override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            return base.UpdateDiscount(request, context);
        }

        public override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            return base.DeleteDiscount(request, context);
        }
    }
}
