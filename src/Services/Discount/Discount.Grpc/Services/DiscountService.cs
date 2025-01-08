using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;
public class DiscountService
(DiscountContext dbContext, ILogger<DiscountService> logger)
: DiscountProtoService.DiscountProtoServiceBase
{
  public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
  {
    var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

    if (coupon == null)
      coupon = new Models.Coupon { ProductName = "Not Discount", Amount = 0, Description = "Not coupon" };

    logger.LogInformation("Discount retrived from {productName} and amount {productAmount}", coupon.ProductName, coupon.Amount);

    return coupon.Adapt<CouponModel>();
  }

  public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
  {
    var coupon = request.Coupon.Adapt<Coupon>();

    if (coupon == null)
      throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid coupon"));

    dbContext.Coupons.Add(coupon);
    await dbContext.SaveChangesAsync();

    logger.LogInformation("Discount is successfully created. ProductName: {productName}", coupon.ProductName);

    return coupon.Adapt<CouponModel>();
  }

  public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
  {
    var coupon = request.Coupon.Adapt<Coupon>();

    if (coupon == null)
      throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid coupon"));

    dbContext.Coupons.Update(coupon);
    await dbContext.SaveChangesAsync();

    logger.LogInformation("Discount is successfully created. ProductName: {productName}", coupon.ProductName);

    return coupon.Adapt<CouponModel>();
  }

  public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
  {
    var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

    if(coupon == null)
      throw new RpcException(new Status(StatusCode.NotFound, $"Invalid coupon: {request.ProductName}"));

    dbContext.Coupons.Remove(coupon);
    await dbContext.SaveChangesAsync();

    logger.LogInformation("Discount is successfully delete. ProductName: {productName}", coupon.ProductName);

    return new DeleteDiscountResponse { Success = true };
  }
}