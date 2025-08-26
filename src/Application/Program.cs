using Application.DependencyInjection.Adapters;
using Application.DependencyInjection.Auth;
using Application.DependencyInjection.Database;
using Application.DependencyInjection.Repositories;
using Application.DependencyInjection.Services;
using Application.DependencyInjection.Swagger;
using Application.DependencyInjection.Validators;
using Application.Driving.Routes;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services
    .AddDatabase(configuration)    
    .AddRepositories()
    .AddServices()
    .AddAdapters()
    .AddValidators()
    .AddAuth(configuration)
    .AddSwagger(); 

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.ConfigureAuthRoutes();
app.ConfigureNewsRoutes();
app.ConfigureUserArticlesRoutes();

await app.RunAsync();