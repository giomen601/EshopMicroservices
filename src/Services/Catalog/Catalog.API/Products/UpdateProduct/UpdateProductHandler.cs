using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using FluentValidation;
using Marten;

namespace Catalog.API.Products.UpdateProduct;
public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price)
: ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
      RuleFor(x => x.Id)
      .NotEmpty()
      .WithMessage("Product Id is required");

      RuleFor(x => x.Name)
      .NotEmpty()
      .WithMessage("Product name is required");
  }
}

internal class UpdateProductHandler
  (IDocumentSession session)
: ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
  public async Task<UpdateProductResult> Handle
  (
    UpdateProductCommand command,
    CancellationToken cancellationToken
  )
  {
    var product = await session.LoadAsync<Product>(command.Id, cancellationToken);

    if (product is null)
    {
      throw new ProductNotFoundException(command.Id);
    }

    product.Name = command.Name;
    product.Description = command.Description;
    product.Price = command.Price;
    product.Category = command.Category;
    product.Description = command.Description;

    session.Update(product);
    await session.SaveChangesAsync();

    return new UpdateProductResult(true);
  }
}