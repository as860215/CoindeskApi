using Coindesk.Controllers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

#pragma warning disable CS8625 // 無法將 null 常值轉換成不可為 Null 的參考型別。單元測試用
namespace Coindesk.Test;

/// <summary>這裡會將HttpClient也Mock掉，並不會有對外連線</summary>
public class CoindeskTest
{
    private readonly CoindeskController coindeskController;

    public CoindeskTest()
    {
        var mockLogger = new Mock<ILogger<CoindeskController>>();
        var mockContext = new Mock<DatabaseContext>();
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();

        mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(new HttpClient(new FakeHttpMessageHandler()));

        mockContext.AddContext<Currency>();

        coindeskController = new CoindeskController(mockLogger.Object, mockContext.Object, mockHttpClientFactory.Object);
    }

    [Fact]
    public void GetCoindesk()
    {
        var result = coindeskController.GetCoindesk();

        result.Should().NotBeNull();
        result!.Time.UpdateTimeIso.Should().Be(new DateTimeOffset(1997, 02, 15, 02, 58, 08, TimeSpan.Zero));
        result.Disclaimer.Should().Be("OuO");
        result.ChartName.Should().Be("RRRRRR你好");
        result.Bpi.Should().HaveCount(4);
        result.Bpi.Should().ContainKey("USD");
        result.Bpi.Should().ContainKey("GBP");
        result.Bpi.Should().ContainKey("XXX");
        result.Bpi.Should().ContainKey("JPY");

        result.Bpi!["USD"].Description.Should().Be("我是美金唷");
        result.Bpi!["USD"].Rate.Should().Be(999888.777m);
        result.Bpi!["GBP"].RateText.Should().Be("79,277.927");
        result.Bpi!["XXX"].Description.Should().Be("啊啊啊啊啊啊我是誰");
        result.Bpi!["XXX"].Currency.Should().Be("AuA");
        result.Bpi!["JPY"].Symbol.Should().Be("Y");
    }

    [Fact]
    public void GetCoindeskUpdateTime()
    {
        var result = coindeskController.GetCoindeskUpdateTime();
        result!.Should().Be("1997/02/15 02:58:08");
    }

    [Fact]
    public void GetCoindeskCurrencyInfo()
    {
        var result = coindeskController.GetCoindeskCurrencyInfo();

        result.Should().NotBeNull();
        result.Should().HaveCount(4);

        const string JPY = nameof(JPY);
        var exceptedJpy = new CurrencyInfoResponse()
        {
            Currency = JPY,
            Name = "日幣",
            Rate = 1548.7m
        };

        result!.FirstOrDefault(n => n.Currency == JPY).Should().NotBeNull();
        result!.First(n => n.Currency == JPY).Should().BeEquivalentTo(exceptedJpy);

        // 因為XXX並不包含在匯率檔內
        const string XXX = nameof(XXX);
        var exceptedXXX = new CurrencyInfoResponse()
        {
            Currency = XXX,
            Name = null,
            Rate = 123m
        };

        result!.FirstOrDefault(n => n.Currency == XXX).Should().NotBeNull();
        result!.First(n => n.Currency == XXX).Should().BeEquivalentTo(exceptedXXX);
    }
}

#pragma warning restore CS8625 // 無法將 null 常值轉換成不可為 Null 的參考型別。單元測試用