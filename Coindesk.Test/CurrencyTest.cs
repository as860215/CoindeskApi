using Coindesk.Controllers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using Namotion.Reflection;
using System.Linq.Expressions;
using Xunit;

#pragma warning disable CS8625 // 無法將 null 常值轉換成不可為 Null 的參考型別。單元測試用
namespace Coindesk.Test;
public class CurrencyTest
{
    private readonly CurrencyController currencyController;

    public CurrencyTest()
    {
        var mockLogger = new Mock<ILogger<CurrencyController>>();
        var mockContext = new Mock<BloggingContext>();

        mockContext.AddContext<Currency>();

        currencyController = new CurrencyController(mockLogger.Object, mockContext.Object);
    }

    [Theory]
    [InlineData("JPY", "日幣")]
    [InlineData("EUR", "歐元")]
    [InlineData("GBP", "英鎊")]
    [InlineData("USD", "美金")]
    public void GetCurrency(string currencyType, string name)
    {
        var result = currencyController.Get(currencyType);

        result.Should().HaveCount(1);
        result.First().Type.Should().Be(currencyType);
        result.First().Name.Should().Be(name);
    }

    [Fact]
    public void AddCurrency_But_Parameter_Null_Should_Throw()
        => FluentActions.Invoking(() => currencyController.Add(null)).Should().Throw<ArgumentNullException>();

    [Fact]
    public void AddCurrency_But_Type_Has_Already_Exists()
        => FluentActions.Invoking(() => currencyController.Add(new Currency("USD", "美金"))).Should().Throw<InvalidOperationException>();

    [Fact]
    public void AddCurrency()
    {
        var currency = new Currency("TWD", "新台幣");
        currencyController.Add(currency);

        
        var result = currencyController.Get(currency.Type);
        result.Should().HaveCount(1);
        result.First().Type.Should().Be(currency.Type);
        result.First().Name.Should().Be(currency.Name);
        result.First().LastUpdateDate.Should().NotBeNull();
    }

    [Fact]
    public void ModifyCurrency_But_Parameter_Null_Should_Throw()
        => FluentActions.Invoking(() => currencyController.Modify(null)).Should().Throw<ArgumentNullException>();

    [Fact]
    public void ModifyCurrency_But_Type_Has_Already_Exists()
        => FluentActions.Invoking(() => currencyController.Modify(new Currency("TWD", "新台幣"))).Should().Throw<InvalidOperationException>();

    [Fact]
    public void ModifyCurrency()
    {
        var beforeModifyTime = DateTimeOffset.Now;
        var currency = new Currency("USD", "dollar");
        currencyController.Modify(currency);

        var result = currencyController.Get(currency.Type);
        result.Should().HaveCount(1);
        result.First().Type.Should().Be(currency.Type);
        result.First().Name.Should().Be(currency.Name);
        result.First().LastUpdateDate.Should().BeAfter(beforeModifyTime);
    }

    [Fact]
    public void DeleteCurrency_But_Parameter_Null_Should_Throw()
        => FluentActions.Invoking(() => currencyController.Delete(null)).Should().Throw<ArgumentNullException>();

    [Fact]
    public void DeleteCurrency_But_Type_Has_Already_Exists()
        => FluentActions.Invoking(() => currencyController.Delete("TWD")).Should().Throw<InvalidOperationException>();

    [Fact]
    public void DeleteCurrency()
    {
        const string currencyType = "USD";
        currencyController.Delete(currencyType);

        var result = currencyController.Get(currencyType);
        result.Should().HaveCount(0);
    }
}
#pragma warning restore CS8625 // 無法將 null 常值轉換成不可為 Null 的參考型別。單元測試用