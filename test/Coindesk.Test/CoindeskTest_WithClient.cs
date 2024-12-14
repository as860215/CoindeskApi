using Coindesk.Controllers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

#pragma warning disable CS8625 // 無法將 null 常值轉換成不可為 Null 的參考型別。單元測試用
namespace Coindesk.Test;

/// <summary>這裡會著重於測試Http連線可行性，故未Mock Http Client</summary>
public class CoindeskTest_WithClient
{
    private readonly CoindeskController coindeskController;

    public CoindeskTest_WithClient()
    {
        var mockLogger = new Mock<ILogger<CoindeskController>>();
        var mockContext = new Mock<DatabaseContext>();

        mockContext.AddContext<Currency>();

        coindeskController = new CoindeskController(mockLogger.Object, mockContext.Object, new EmptyHttpClient());
    }

    [Fact]
    public void GetCoindesk()
    {
        var result = coindeskController.GetCoindesk();
        result.Should().NotBeNull();
    }

    [Fact]
    public void GetCoindeskUpdateTime()
    {
        var result = coindeskController.GetCoindeskUpdateTime();
        result.Should().NotBeNull();
    }

    [Fact]
    public void GetCoindeskCurrencyInfo()
    {
        var result = coindeskController.GetCoindeskCurrencyInfo();
        result.Should().NotBeNull();
    }
}
public class EmptyHttpClient : IHttpClientFactory
{
    public HttpClient CreateClient(string name) => new HttpClient();
}
#pragma warning restore CS8625 // 無法將 null 常值轉換成不可為 Null 的參考型別。單元測試用