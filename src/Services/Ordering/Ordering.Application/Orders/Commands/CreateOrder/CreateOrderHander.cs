using BuildingBlocks.CQRS;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders.Commands.CreateOrder;
public class CreateOrderHander(IApplicationDbContext dbContext)
: ICommandHandler<CreateOrderCommand, CreatedOrderResult>
{
  public async Task<CreatedOrderResult> Handle
  (
    CreateOrderCommand command,
    CancellationToken cancellationToken
  )
  {
    var order = CreateNewOrder(command.Order);

    dbContext.Orders.Add(order);
    await dbContext.SaveChangesAsync( cancellationToken );

    return new CreatedOrderResult( order.Id.Value );
  }

  private Order CreateNewOrder(OrderDto orderDto)
  {
    var shippingAddress = Address.Of
    (
      orderDto.ShippingAddress.FirstName,
      orderDto.ShippingAddress.LastName,
      orderDto.ShippingAddress.EmailAddress,
      orderDto.ShippingAddress.AddressLine,
      orderDto.ShippingAddress.Country,
      orderDto.ShippingAddress.State,
      orderDto.ShippingAddress.ZipCode
    );

    var billingAddress = Address.Of
    (
      orderDto.BillingAdress.FirstName,
      orderDto.BillingAdress.LastName,
      orderDto.BillingAdress.EmailAddress,
      orderDto.BillingAdress.AddressLine,
      orderDto.BillingAdress.Country,
      orderDto.BillingAdress.State,
      orderDto.BillingAdress.ZipCode
    );

    var orderToCreate = Order.Create
    (
      id: OrderId.Of(Guid.NewGuid()),
      customerId: CustomerId.Of(orderDto.CustomerId),
      orderName: OrderName.Of(orderDto.OrderName),
      shippingAddress: shippingAddress,
      billingAddress: billingAddress,
      payment: Payment.Of
      (
        orderDto.Payment.CardName,
        orderDto.Payment.CardNumber,
        orderDto.Payment.Expiration,
        orderDto.Payment.Cvv,
        orderDto.Payment.PaymentMethod
      )
    );

    foreach (var orderItemDto in orderDto.OrderItems)
    {
      orderToCreate.Add(ProductId.Of(orderItemDto.ProductId), orderItemDto.Quantity, orderItemDto.Price);
    }

    return orderToCreate;
  }
}