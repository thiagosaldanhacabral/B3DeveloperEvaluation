using B3DeveloperEvaluation.Application.Commands;
using B3DeveloperEvaluation.Application.Dtos;
using B3DeveloperEvaluation.Application.Interfaces;
using B3DeveloperEvaluation.Application.Mappings;
using B3DeveloperEvaluation.Application.Options;
using B3DeveloperEvaluation.Application.Services;
using MediatR;
using Serilog;
using System.Reflection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Options configuration (binds InvestmentRatesOptions to appsettings.json)
builder.Services.Configure<InvestmentRatesOptions>(builder.Configuration);

// Logger configuration and registration
var logger = new LoggerConfiguration()
    .WriteTo.File("logs/api-log-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

Log.Logger = logger;

builder.Host.UseSerilog(logger);

// Domain services registration
builder.Services.AddScoped<ICalcInvestmentService, CalcInvestmentService>();

// MediatR registration
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CalculateInvestmentCommand).Assembly));

// AutoMapper registration (using assembly for easier profile management)
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(InvestmentProfile))!);

// Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "B3 Developer Evaluation API", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Main endpoint for investment calculation
app.MapPost("/calculate", async (InvestmentRequestDto req, IMediator mediator, ILogger<Program> logger) =>
{
    if (req == null || req.Amount <= 0 || req.Months <= 0)
    {
        logger.LogWarning("Invalid request: {@Request}", req);
        return Results.BadRequest("Invalid request: Amount and Months must be greater than zero.");
    }

    try
    {
        var response = await mediator.Send(new CalculateInvestmentCommand(req.Amount, req.Months));
        logger.LogInformation("Calculation successful for Amount={Amount}, Months={Months}", req.Amount, req.Months);
        return Results.Ok(response);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error processing request for Amount={Amount}, Months={Months}", req.Amount, req.Months);
        return Results.Problem("An unexpected error occurred while processing your request.", statusCode: 500);
    }
})
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status500InternalServerError)
.WithName("Calculate");

await app.RunAsync();