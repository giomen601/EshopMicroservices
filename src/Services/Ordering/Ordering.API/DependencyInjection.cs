using BuildingBlocks.Exceptions.Handler;
using Carter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;

namespace Ordering.API;
public static class DependencyInjection
{
  public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddCarter();
    services.AddExceptionHandler<CustomExceptionHandler>(); //Use custome exceptions
    services.AddHealthChecks();

    //services.AddHealthChecks().AddSqlServer(configuration.GetConnectionString("Database")!);
    return services;
  }

  public static WebApplication UseApiServices(this WebApplication app)
  {
    app.MapCarter();

    app.UseExceptionHandler(op => { });//Use custome exceptions
    app.UseHealthChecks("/health");
    /*app.UseHealthChecks("/health",
      new HealthCheckOptions
      {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
      });*/

    return app;
  }
}