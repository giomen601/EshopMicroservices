using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Data;
using Ordering.Infraestructure.Data;
using Ordering.Infraestructure.Data.Interceptor;

namespace Ordering.Infraestructure;
public static class DependencyInjection
{
  public static IServiceCollection AddInfraestructueServices
  (
    this IServiceCollection services,
    IConfiguration configuration
  )
  {
    var connectionString = configuration.GetConnectionString("Database");

    //Add services
    services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
    services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();

    

    services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
    {
      //options.AddInterceptors(new AuditableEntityInterceptor(),
      //  new DispatchDomainEventInterceptor());
      options.AddInterceptors(serviceProvider.GetService<ISaveChangesInterceptor>());
      options.UseSqlServer(connectionString);
    });

    services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

    return services;
  }
}