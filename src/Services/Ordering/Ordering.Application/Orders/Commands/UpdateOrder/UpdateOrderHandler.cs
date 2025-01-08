using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Application.Exceptions;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Commands.UpdateOrder;
public class UpdateOrderHandler(IApplicationDbContext dbContext)
: ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
{
  public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
  {
    var orderId = OrderId.Of(command.Order.Id);
    var order = await dbContext.Orders
      .FindAsync([orderId], cancellationToken: cancellationToken);

    if(order == null)
      throw new OrderNotFoundException(command.Order.Id);

    UpdateOrderWithNewValues(order, command.Order);

    dbContext.Orders.Update(order);
    await dbContext.SaveChangesAsync(cancellationToken);

    return new UpdateOrderResult(true);
  }

  private void UpdateOrderWithNewValues(Order order, OrderDto orderDto)
  {
    var updatedShippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName, orderDto.ShippingAddress.EmailAddress, orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.Country, orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);
    var updatedBillingAddress = Address.Of(orderDto.BillingAdress.FirstName, orderDto.BillingAdress.LastName, orderDto.BillingAdress.EmailAddress, orderDto.BillingAdress.AddressLine, orderDto.BillingAdress.Country, orderDto.BillingAdress.State, orderDto.BillingAdress.ZipCode);
    var updatedPayment = Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.Expiration, orderDto.Payment.Cvv, orderDto.Payment.PaymentMethod);

    order.Update(
            orderName: OrderName.Of(orderDto.OrderName),
            shippingAddress: updatedShippingAddress,
            billingAddress: updatedBillingAddress,
            payment: updatedPayment,
            status: orderDto.Status);
  }
}