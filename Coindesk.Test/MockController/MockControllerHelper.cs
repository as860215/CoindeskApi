using Moq;

namespace Coindesk.Test;

/// <summary>Mock Controllere工廠</summary>
/// <remarks>為以後可能擴充更多的類別預留設計空間</remarks>
public static class MockControllerHelper
{
    /// <summary>新增上下文</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="context">上下文</param>
    /// <returns>上下文</returns>
    public static Mock<BloggingContext> AddContext<T>(this Mock<BloggingContext> context) where T : class
    {
        MockControllerBase? mockController = null;
        if (typeof(T) == typeof(Currency))
            mockController = new MockCurrency();

        mockController?.Create(context);

        return context;
    }
}
