using Basket.API.Dtos;
using Carter;
using Mapster;
using MediatR;

namespace Basket.API.Basket.CheckoutBasket;
public record CheckoutBasketRequest(BasketCheckoutDto BasketCheckoutDto);
public record CheckoutBasketResponse(bool IsSuccess);
public class CheckoutBasketEndpoints : ICarterModule
{
  public void AddRoutes(IEndpointRouteBuilder app)
  {
    app.MapPost("/bascket/checkout", async (CheckoutBasketRequest request, ISender sender) =>
    {
      var command = request.Adapt<CheckoutBasketCommand>();

      var result = await sender.Send(command);

      var response = result.Adapt<CheckoutBasketResponse>();

      return Results.Ok(response);
    });
  }
}