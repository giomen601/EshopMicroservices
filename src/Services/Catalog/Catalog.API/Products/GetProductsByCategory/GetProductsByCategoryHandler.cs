using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten;
using Marten.Linq.QueryHandlers;

namespace Catalog.API.Products.GetProductsByCategory;
public record GetProductsByCategoryQuery(string Category) : IQuery<GetProductsByCategoryResult>;
public record GetProductsByCategoryResult(IEnumerable<Product> products);
internal class GetProductsByCategoryHandler
  (
    IDocumentSession session,
    ILogger<GetProductsByCategoryHandler> logger
  )
: IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
  public async Task<GetProductsByCategoryResult> Handle
  (
    GetProductsByCategoryQuery query,
    CancellationToken cancellationToken
  )
  {
    var products = await session.Query<Product>()
      .Where(p => p.Category.Contains(query.Category))
      .ToListAsync(cancellationToken);

    return new GetProductsByCategoryResult(products);
  }
}