using B3DeveloperEvaluation.Application.Options;
using B3DeveloperEvaluation.Application.Services;
using Microsoft.Extensions.Options;

namespace B3DeveloperEvaluation.Api.Tests;

public class CalcInvestmentServiceTests
{
    private static CalcInvestmentService CreateService(decimal cdi = 0.009m, decimal tb = 1.08m)
    {
        var options = Options.Create(new InvestmentRatesOptions { Cdi = cdi, Tb = tb });
        return new CalcInvestmentService(options);
    }

    [Theory]
    [InlineData(1000, 1)]
    [InlineData(1000, 6)]
    [InlineData(1000, 12)]
    [InlineData(1000, 24)]
    [InlineData(1000, 36)]
    public void CalculateReturn_ShouldReturnExpectedValues(decimal initial, int months)
    {
        var svc = CreateService();
        var result = svc.CalculateReturn(initial, months);

        Assert.NotNull(result);
        Assert.True(result.GrossAmount > 0);
        Assert.True(result.NetAmount > 0);
        Assert.True(result.TaxAmount >= 0);
        Assert.True(result.GrossAmount > result.NetAmount);
    }

    [Theory]
    [InlineData(0, 12)]
    [InlineData(1000, 0)]
    [InlineData(-100, 12)]
    [InlineData(1000, -5)]
    public void CalculateReturn_ShouldThrowArgumentException_WhenInvalidInput(decimal initial, int months)
    {
        var svc = CreateService();
        Assert.Throws<ArgumentException>(() => svc.CalculateReturn(initial, months));
    }

    [Theory]
    [InlineData(1000, 6, 0.225)]
    [InlineData(1000, 12, 0.20)]
    [InlineData(1000, 24, 0.175)]
    [InlineData(1000, 36, 0.15)]
    public void CalculateReturn_TaxRateIsCorrect(decimal initial, int months, double expectedRate)
    {
        var svc = CreateService();
        var result = svc.CalculateReturn(initial, months);

        var grossProfit = result.GrossAmount - initial;
        var expectedTax = grossProfit * (decimal)expectedRate;
        Assert.Equal(decimal.Round(expectedTax, 2), result.TaxAmount);
    }

    [Fact]
    public void CalculateReturn_ShouldBeConsistent_ForSameInput()
    {
        var svc = CreateService();
        var result1 = svc.CalculateReturn(1000, 12);
        var result2 = svc.CalculateReturn(1000, 12);

        Assert.Equal(result1.GrossAmount, result2.GrossAmount);
        Assert.Equal(result1.NetAmount, result2.NetAmount);
        Assert.Equal(result1.TaxAmount, result2.TaxAmount);
    }
}
