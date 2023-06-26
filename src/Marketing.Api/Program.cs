using System;
using System.Collections.Generic;
using System.Threading;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hellang.Middleware.ProblemDetails;
using Marketing.Api.Middleware;
using Marketing.Api.Modules;
using Marketing.Api.Security;
using Marketing.Application.Domain;
using Marketing.Application.Modules;
using Marketing.Infrastructure.Domain;
using Marketing.Infrastructure.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Formatting.Json;
using Serilog.Sinks.File;
using Swashbuckle.AspNetCore.SwaggerGen;

var host = WebApplication.CreateBuilder();

// Add Autofac services
host.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
host.Host.ConfigureContainer((HostBuilderContext context, ContainerBuilder container) =>
{
    container.Register(_ => context.Configuration).SingleInstance().As<IConfiguration>();
    container.Register(_ => context.HostingEnvironment).SingleInstance().As<IHostEnvironment>();
    container.RegisterModule<ApiModule>();
    container.RegisterModule<ApplicationModule>();
    container.RegisterModule<InfrastructureModule>();
});

// Add application services
host.Services.AddControllers();
host.Services.AddMvc().AddControllersAsServices();
host.Services.AddProblemDetails(options =>
{
    options.IncludeExceptionDetails = (context, _) =>
    {
        var environment = context.RequestServices.GetRequiredService<IHostEnvironment>();
        return environment.IsDevelopment();
    };
});
host.Services.AddAuthentication("Bearer")
    .AddScheme<BearerTokenAuthenticationOptions, BearerTokenAuthenticationHandler>("Bearer", options =>
    {
        var tokens = host.Configuration.GetSection("GlobalBearerTokens").Get<List<GlobalBearerToken>>();
        options.GlobalBearerTokens.AddRange(tokens!);
    });

if (host.Environment.IsDevelopment())
{
    host.Services.AddEndpointsApiExplorer();
    host.Services.AddSwaggerGen(o =>
    {
        o.SwaggerDoc("v1", new OpenApiInfo { Title = "Marketing API", Version = "v1" });
    });
}

// Add additional application configuration
host.Configuration.AddJsonFile("logging.json", optional: false);
host.Configuration.AddJsonFile(@$"logging.{host.Environment.EnvironmentName}.json", optional: false);

// Configure application pipeline
var app = host.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(ui =>
    {
        ui.DocumentTitle = "Marketing API";
        ui.SwaggerEndpoint("/swagger/v1/swagger.json", "Marketing API");
    });
    app.UseDeveloperExceptionPage();
}

app.UseProblemDetails();
app.UseMiddleware<IntakeMiddleware>(
    app.Services.GetService<IIntakeRepository>(),
    app.Services.GetService<INotificationService>(),
    app.Services.GetAutofacRoot().ResolveNamed<AsyncLocal<int?>>("IntakeId"));
app.UseMiddleware<ErrorHandlingMiddleware>(app.Services.GetService<ILogger>());
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

