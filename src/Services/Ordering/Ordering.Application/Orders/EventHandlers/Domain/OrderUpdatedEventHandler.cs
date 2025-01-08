using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Events;

namespace Ordering.Application.Orders.EventHandlers.Domain;
public class OrderUpdatedEventHandler(ILogger<OrderUpdatedEventHandler> logger)
: INotificationHandler<OrderUpdateEvent>
{
  public Task Handle(OrderUpdateEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation("Domain event handler: {DomainEvent}", notification.GetType().Name);
    return Task.CompletedTask;
  }
}