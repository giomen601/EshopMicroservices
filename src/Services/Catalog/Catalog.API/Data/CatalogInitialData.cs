using Catalog.API.Models;
using Marten;
using Marten.Schema;

namespace Catalog.API.Data;
public class CatalogInitialData : IInitialData
{
  public async Task Populate(IDocumentStore store, CancellationToken cancellation)
  {
    using var session = store.LightweightSession();

    if(await session.Query<Product>().AnyAsync()) 
      return;

    //Marten UPSET will cater for existing records
    session.Store<Product>(GetPreconfiguredProducts());
    await session.SaveChangesAsync();
  }

  private static IEnumerable<Product> GetPreconfiguredProducts() => new List<Product>
  {
    new Product
    {
      Id = new Guid("e7894fa5-069d-4da1-a143-6ee36fd971f4"),
      Name = "Test",
      Description = "Test description",
      ImageFile = "product-1.png",
      Price = 950.00M,
      Category = new List<string>{ "Smart Phone" }
    }
  };
}