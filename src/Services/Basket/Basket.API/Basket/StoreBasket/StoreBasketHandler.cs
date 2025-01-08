using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.CQRS;
using Discount.Grpc;
using FluentValidation;

namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart)
        .NotNull()
        .WithMessage("{PropertyName} must not be null");
    }
}
public class StoreBasketCommandHandler
(
  IBasketRepository basketRepository,
  DiscountProtoService.DiscountProtoServiceClient discountProto
)
: ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
  public async Task<StoreBasketResult> Handle
  (
    StoreBasketCommand command,
    CancellationToken cancellationToken
  )
  {
    //Communicate with GRPC discount
    await DeductDiscount(command.Cart, cancellationToken);

    //TODO: i'm lazzy to write
    //TODO: update chache
    await basketRepository.StoreBasket(command.Cart, cancellationToken );

    return new StoreBasketResult(command.Cart.UserName);
  }

  private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
  {
    foreach (var item in cart.Items)
    {
      var coupon = await discountProto.GetDiscountAsync
        (
          new GetDiscountRequest { ProductName = item.ProductName },
          cancellationToken: cancellationToken
        );
      item.Price -= coupon.Amount;
    }
  }
}