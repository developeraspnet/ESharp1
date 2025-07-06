using Catalog.API.Products.CreateProduct;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services.AddCarter();

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();

app.Run();