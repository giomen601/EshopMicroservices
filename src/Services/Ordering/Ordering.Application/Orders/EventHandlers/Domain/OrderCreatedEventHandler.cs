using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Ordering.Application.Extensions;
using Ordering.Domain.Events;

namespace Ordering.Application.Orders.EventHandlers.Domain;
public class OrderCreatedEventHandler
(IPublishEndpoint publishEndpoint, IFeatureManager featureManager, ILogger<OrderCreatedEventHandler> logger)
: INotificationHandler<OrderCreateEvent>
{
  public async Task Handle(OrderCreateEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation("Domain event handler: {DomainEvent}", domainEvent.GetType().Name);
    
    if(await featureManager.IsEnabledAsync("OrderFullfilment"))
    {
      var orderCreatedIntegrationEvent = domainEvent.order.ToOrderDto();
      await publishEndpoint.Publish(orderCreatedIntegrationEvent, cancellationToken);
    }
  }
}