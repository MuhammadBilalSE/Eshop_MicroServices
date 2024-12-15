using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Discount.Grpc.Protos;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Discount.Grpc.Services
{
	public class DiscountService(DiscountContext dbcontext, ILogger<DiscountService> logger)
		: DiscountProtoService.DiscountProtoServiceBase
	{
		public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
		{
			if(request is not  null )
			{
				var coupon = request.Coupon.Adapt<Coupon>();
				dbcontext.Coupons.Add(coupon);
				await dbcontext.SaveChangesAsync();
				logger.LogInformation($"Discount Created for Product:{coupon.ProductName}, Amount {coupon.Amount}");
			}
			throw new RpcException(new Status(StatusCode.InvalidArgument,"Invalid Request"));
		}

		public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
		{
			var coupon = await dbcontext.Coupons.FirstOrDefaultAsync(
				 x => x.ProductName == request.ProductName);
			if (coupon is null)
				coupon = new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };
			logger.LogInformation($"Discount Applied for Product:{coupon.ProductName}, Amount {coupon.Amount}");
			var couponmodel = coupon.Adapt<CouponModel>();
			return couponmodel;
		}

		public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
		{
			var coupon= request.Coupon.Adapt<Coupon>();
			if (coupon is null)
				throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request"));
			dbcontext.Coupons.Update(coupon);
			await dbcontext.SaveChangesAsync();
			logger.LogInformation($"Discount Updated for Product:{coupon.ProductName}, Amount {coupon.Amount}");
			return coupon.Adapt<CouponModel>();
		}

		public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
		{
			var coupon = await dbcontext.Coupons.FirstOrDefaultAsync(
				 x => x.ProductName == request.ProductName);
			if (coupon is null)
				throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request"));
			dbcontext.Coupons.Remove(coupon);
			await dbcontext.SaveChangesAsync();
			logger.LogInformation($"Discount Deleted for Product:{coupon.ProductName}");
			return new DeleteDiscountResponse { Success = true };
		}
	} 
}
