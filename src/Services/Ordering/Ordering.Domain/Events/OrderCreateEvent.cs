using Ordering.Domain.Abstranctions;
using Ordering.Domain.Models;

namespace Ordering.Domain.Events;
public record OrderCreateEvent(Order order) : IDomainEvent;