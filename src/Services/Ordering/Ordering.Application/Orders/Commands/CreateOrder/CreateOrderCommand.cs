using BuildingBlocks.CQRS;
using FluentValidation;
using Ordering.Application.Dtos;

namespace Ordering.Application.Orders.Commands.CreateOrder;
public record CreateOrderCommand(OrderDto Order)
: ICommand<CreatedOrderResult>;

public record CreatedOrderResult(Guid Id);

public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
  public CreateOrderValidator()
  {
    RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Name is required");
  }
}