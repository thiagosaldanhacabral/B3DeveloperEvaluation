using B3DeveloperEvaluation.Api.Dtos;
using B3DeveloperEvaluation.Application.Interfaces;
using B3DeveloperEvaluation.Application.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICalcInvestmentService, CalcInvestmentService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "B3 Developer Evaluation API", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/calculate", (InvestmentRequestDto req, ICalcInvestmentService svc) =>
{
    var inv = new B3DeveloperEvaluation.Domain.Entities.Investment { Amount = req.Amount, Months = req.Months };
    var gross = svc.CalculateGross(inv);
    var net = svc.CalculateNet(inv);
    return Results.Ok(new InvestmentResponseDto { Gross = gross, Net = net });
});

await app.RunAsync();
