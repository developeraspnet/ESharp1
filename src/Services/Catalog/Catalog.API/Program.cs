using Catalog.API.Products.CreateProduct;
using Marten;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services.AddCarter();

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddMarten(configure =>
{
    configure.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();

app.Run();