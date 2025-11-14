using FormCollector.Application;
using FormCollector.Infrastructure;
using MinimalApi.Endpoint.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services
    .AddApplication()
    .AddInfrastructure();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapEndpoints();

app.Run();
