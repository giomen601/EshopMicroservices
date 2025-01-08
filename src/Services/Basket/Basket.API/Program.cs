using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Messaging.MassTransit;
using Carter;
using Discount.Grpc;
using HealthChecks.UI.Client;
using Marten;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container

#region Application Services
var assembly = typeof(Program).Assembly;


builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
  config.RegisterServicesFromAssembly(assembly);
  config.AddOpenBehavior(typeof(ValidationBehavior<,>));
  config.AddOpenBehavior(typeof(LogginBeheavios<,>));
});
#endregion

#region Data Services
builder.Services.AddMarten(opts =>
{
  opts.Connection(builder.Configuration.GetConnectionString("Database")!);
  opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

/*builder.Services.AddScoped<IBasketRepository>(provider =>
{
  var basketRepository = provider.GetRequiredService<BasketRepository>();

  return new CachedBasketRepository(basketRepository, provider.GetRequiredService<IDistributedCache>());
});*/ //MANUALY BEFORE ADD SCRUPTOR NUGGET
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>(); //Decorate commes from SCRUPTOR

builder.Services.AddStackExchangeRedisCache(options =>
{
  options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
#endregion

#region GRPC Services
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
  options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);

})
  //SSL certification error solved (use only in develop env)
.ConfigurePrimaryHttpMessageHandler(() =>
{
  var handler = new HttpClientHandler
  {
    ServerCertificateCustomValidationCallback =
    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
  };

  return handler;
});
#endregion

#region register mass transient rabbitmq configuration
//Async communication services
builder.Services.AddMessageBroker(builder.Configuration);
#endregion

#region Cross-Cutting Services
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
  .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
  .AddRedis(builder.Configuration.GetConnectionString("Redis")!);
#endregion

var app = builder.Build();

//Configure the HTTP request pipeline
app.MapCarter();
app.UseExceptionHandler(options =>
{

});

app.UseHealthChecks("/health", 
  new HealthCheckOptions
  {
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
  });

app.Run();
