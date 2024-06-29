using CustomerTestApp.Service;
using CustomerTestApp.Service.Interceptors;
using CustomerTestApp.Service.Models;
using CustomerTestApp.Service.Repositories;
using CustomerTestApp.Service.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<LoggingInterceptor>();
});
builder.Services.AddGrpcReflection();
builder.Services.AddDbContext<CustomerContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("CustomerTestDatabase")));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddTransient<LoggingInterceptor>();

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<CustomerContext>();
        context.Database.Migrate();
        InitializeDB.Initialize(context);
    }
    catch (Exception ex)
    {
    }
}

app.MapGrpcService<CustomerService>();
app.MapGrpcReflectionService();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
