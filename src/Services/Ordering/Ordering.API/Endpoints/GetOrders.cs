using BuildingBlocks.Pagination;
using Carter;
using Mapster;
using MediatR;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Queries.GetOrders;

namespace Ordering.API.Endpoints;
public record GetOrdersResponse(PaginatedResult<OrderDto> Orders);
public class GetOrders : ICarterModule
{
  public void AddRoutes(IEndpointRouteBuilder app)
  {
    app.MapGet("/orders", async ([AsParameters] PaginationRequest reques, ISender sender) =>
    {
      var result = await sender.Send(new GetOrdersQuery(reques));
      var response = result.Adapt<GetOrdersResponse>();
      return Results.Ok(response);
    })
    .WithName("GetOrders")
    .Produces<CreateOrderResponse>(StatusCodes.Status200OK)
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .ProducesProblem(StatusCodes.Status404NotFound)
    .WithSummary("GetOrders")
    .WithDescription("GetOrders");
  }
}