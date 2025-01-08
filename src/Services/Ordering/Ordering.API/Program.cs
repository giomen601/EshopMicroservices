using Ordering.API;
using Ordering.Application;
using Ordering.Infraestructure;
using Ordering.Infraestructure.Data.Extentions;

var builder = WebApplication.CreateBuilder(args);

//add services
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfraestructueServices(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

//Configure the HTTP request pipeline
app.UseApiServices();

if (app.Environment.IsDevelopment())
{
  await app.InitialiceDbAsync();
}

app.Run();


//MIGRATION COMMAND
//add-migration initial -OutputDir Data/Migrations -Project Ordering.Infraestructure -StartupProject Ordering.API