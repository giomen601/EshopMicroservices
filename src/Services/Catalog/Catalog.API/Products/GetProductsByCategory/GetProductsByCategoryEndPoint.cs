using Carter;
using Catalog.API.Models;
using Catalog.API.Products.CreateProduct;
using Mapster;
using MediatR;

namespace Catalog.API.Products.GetProductsByCategory;
public record GetProductByCategoryResponse(IEnumerable<Product> products);
public class GetProductsByCategoryEndPoint : ICarterModule
{
  public void AddRoutes(IEndpointRouteBuilder app)
  {
    app.MapGet("/products/category/{category}", async (string category, ISender sender) =>
    {
      var result = await sender.Send(new GetProductsByCategoryQuery(category));
      var response = result.Adapt<GetProductByCategoryResponse>();
      return Results.Ok(response);
    })
     .WithName("GetProductByCategory")
    .Produces<CreateProductResponse>(StatusCodes.Status200OK)
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .WithSummary("Get Product by category")
    .WithDescription("Get Product by category"); ;
  }
}