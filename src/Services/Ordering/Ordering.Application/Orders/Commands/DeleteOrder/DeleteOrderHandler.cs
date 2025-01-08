using BuildingBlocks.CQRS;
using Ordering.Application.Data;
using Ordering.Application.Exceptions;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders.Commands.DeleteOrder;
public class DeleteOrderHandler(IApplicationDbContext dbContext)
  : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
{
  public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
  {
    var oderId = OrderId.Of(command.OrderId);
    var order = await dbContext.Orders
      .FindAsync([oderId], cancellationToken: cancellationToken);

    if (order == null)
      throw new OrderNotFoundException(command.OrderId);

    dbContext.Orders.Remove(order);
    await dbContext.SaveChangesAsync(cancellationToken);

    return new DeleteOrderResult(true);
  }
}