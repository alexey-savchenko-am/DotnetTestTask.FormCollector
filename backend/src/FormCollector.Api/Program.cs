using FormCollector.Application;
using FormCollector.Infrastructure;
using FormCollector.Infrastructure.Data;
using MinimalApi.Endpoint.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;


Console.WriteLine("Helllllllllllllllllllllooooo");
var configuration = builder.Configuration;
Console.WriteLine(configuration.GetConnectionString("FormCollectorDb"));

services
    .AddApplication()
    .AddInfrastructure();

services.AddEndpoints();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var databaseInitializer = scope.ServiceProvider.GetService<IDatabaseInitializer>();
    databaseInitializer?.InitializeAsync(recreateDatabase: true).Wait();
}

app.UseHttpsRedirection();

app.Run();
