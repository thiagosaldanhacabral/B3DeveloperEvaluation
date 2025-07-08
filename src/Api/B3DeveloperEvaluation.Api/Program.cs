using B3DeveloperEvaluation.Application.Commands;
using B3DeveloperEvaluation.Application.Dtos;
using B3DeveloperEvaluation.Application.Interfaces;
using B3DeveloperEvaluation.Application.Mappings;
using B3DeveloperEvaluation.Application.Options;
using B3DeveloperEvaluation.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

static void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.Configure<InvestmentRatesOptions>(builder.Configuration)
                    .AddScoped<ICalcInvestmentService, CalcInvestmentService>()
                    .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CalculateInvestmentCommand).Assembly))
                    .AddAutoMapper(Assembly.GetAssembly(typeof(InvestmentProfile))!)
                    .AddEndpointsApiExplorer()
                    .AddSwaggerGen(c =>
                    {
                        c.SwaggerDoc("v1", new OpenApiInfo { Title = "B3 Developer Evaluation API", Version = "v1" });
                    })
                    .AddCors(options =>
                    {
                        options.AddPolicy("AllowAll", policy =>
                        {
                            policy.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader();
                        });
                    })
                    .AddHealthChecks();
}

// Configura��o do logger Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/api-log-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger);

// Configura��o dos servi�os
ConfigureServices(builder);

var app = builder.Build();

// Configura��o dos middlewares
app.UseCors("AllowAll")
   .UseSwagger()
   .UseSwaggerUI();

// Endpoint de health check simplificado (para Docker)
app.MapGet("/health/ready", () => Results.Ok(new { status = "ready", timestamp = DateTime.UtcNow, service = "B3DeveloperEvaluation.Api" }));

// Endpoint de health check para liveness probe
app.MapGet("/health/live", () => Results.Ok(new { status = "alive", timestamp = DateTime.UtcNow, service = "B3DeveloperEvaluation.Api" }));

// Endpoint principal para c�lculo de investimento
app.MapPost("/calculate", async (InvestmentRequestDto req, IMediator mediator, ILogger<Program> logger) =>
{
    // Valida��o dos par�metros de entrada
    if (req is not { Amount: > 0, Months: > 0 })
    {
        logger.LogWarning("Requisi��o inv�lida: {@Request}", req);
        return Results.BadRequest("Requisi��o inv�lida: Amount e Months devem ser maiores que zero.");
    }

    try
    {
        var response = await mediator.Send(new CalculateInvestmentCommand(req.Amount, req.Months));
        logger.LogInformation("C�lculo realizado com sucesso para Amount={Amount}, Months={Months}", req.Amount, req.Months);
        return Results.Ok(response);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Erro ao processar a requisi��o para Amount={Amount}, Months={Months}", req.Amount, req.Months);
        return Results.Problem("Ocorreu um erro inesperado ao processar sua requisi��o.", statusCode: 500);
    }
})
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status500InternalServerError)
.WithName("Calculate");

await app.RunAsync();