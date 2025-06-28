namespace B3DeveloperEvaluation.Api.Dtos
{
    public record InvestmentResponseDto
    {
        public decimal Gross { get; init; }
        public decimal Net { get; init; }
    }
}
