using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using Marten;
using Marten.Linq.QueryHandlers;

namespace Catalog.API.Products.GetProductById;
public record GetProductByIdQuery(Guid id) : IQuery<GetProductByIdResult>;
public record GetProductByIdResult(Product product);
internal class GetProductByIdQueryHandler
(
  IDocumentSession session,
  ILogger<GetProductByIdQueryHandler> logger
) :
IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
  public async Task<GetProductByIdResult> Handle
  (
    GetProductByIdQuery query,
    CancellationToken cancellationToken
  )
  {
    logger.LogInformation("GetProductByIdQueryHandler {@Query}", query);

    var product = await session.LoadAsync<Product>( query.id, cancellationToken );

    if(product == null )
    {
      throw new ProductNotFoundException(query.id);
    }

    return new GetProductByIdResult(product);
  }
}