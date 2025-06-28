using B3DeveloperEvaluation.Application.Commands;
using B3DeveloperEvaluation.Application.Dtos;
using B3DeveloperEvaluation.Application.Interfaces;
using B3DeveloperEvaluation.Application.Mappings;
using B3DeveloperEvaluation.Application.Services;
using MediatR;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICalcInvestmentService, CalcInvestmentService>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CalculateInvestmentCommand).Assembly));

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<InvestmentProfile>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "B3 Developer Evaluation API", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/calculate", async (InvestmentRequestDto req, IMediator mediator) =>
{
    var response = await mediator.Send(new CalculateInvestmentCommand(req.Amount, req.Months));
    return Results.Ok(response);
});

await app.RunAsync();