using Carter;
using Mapster;
using MediatR;

namespace Catalog.API.Products.DeleteProduct;
public record DeleteProductResponse(bool isSuccess);
public class DeleteProductEndPoint : ICarterModule
{
  public void AddRoutes(IEndpointRouteBuilder app)
  {
    app.MapDelete("/products/{id}", async (Guid id, ISender sender) =>
    {
      var result = sender.Send(new DeleteProductCommand(id));

      var response = result.Adapt<DeleteProductResponse>();

      return Results.Ok(response);
    });
  }
}