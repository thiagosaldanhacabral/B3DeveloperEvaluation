namespace B3DeveloperEvaluation.Application.Dtos;

public class InvestmentResponseDto
{
    public decimal GrossAmount { get; init; }
    public decimal NetAmount { get; init; }
    public decimal TaxAmount { get; set; }
}
