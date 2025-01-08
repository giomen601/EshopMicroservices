
using BuildingBlocks.CQRS;
using Catalog.API.Models;
using FluentValidation;
using Marten;

namespace Catalog.API.Products.DeleteProduct;
public record DeleteProductCommand(Guid id) : ICommand<DeleteProductResult>;
public record DeleteProductResult(bool isSuccess);

public class DeleteProductCommandValidation : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidation()
    {
        RuleFor(x => x.id).NotEmpty();
    }
}
internal class DeleteProductCommandHandler
  (IDocumentSession session)
  : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
  public async Task<DeleteProductResult> Handle
  (
    DeleteProductCommand command,
    CancellationToken cancellationToken
  )
  {
    session.Delete<Product>( command.id );
    await session.SaveChangesAsync(cancellationToken);
    return new DeleteProductResult(true);
  }
}