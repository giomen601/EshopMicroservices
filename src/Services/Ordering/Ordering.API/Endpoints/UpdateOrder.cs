using Carter;
using Mapster;
using MediatR;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Commands.UpdateOrder;

namespace Ordering.API.Endpoints;
public record UpdateOrdedrRequest(OrderDto OrderDto);
public record UpdateOrderResponse(bool IsSuccess);
public class UpdateOrder : ICarterModule
{
  public void AddRoutes(IEndpointRouteBuilder app)
  {
    app.Map("/orders", async (UpdateOrdedrRequest request, ISender sender) =>
    {
      var command = request.Adapt<UpdateOrderCommand>();

      var result = await sender.Send(command);

      var response = result.Adapt<UpdateOrderResponse>();

      return Results.Ok(response);
    })
    .WithName("UpdateOrder")
    .Produces<CreateOrderResponse>(StatusCodes.Status200OK)
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .WithSummary("Update Order")
    .WithDescription("Update Order");
  }
}