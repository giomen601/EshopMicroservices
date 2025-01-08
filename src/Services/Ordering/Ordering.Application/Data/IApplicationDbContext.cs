using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Models;

namespace Ordering.Application.Data;
public interface IApplicationDbContext
{
  DbSet<Customer> Customers { get; set; }
  DbSet<Product> Products { get; set; }
  DbSet<Order> Orders { get; set;}
  DbSet<OrderItem> OrdersItems { get; set; }

  Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}