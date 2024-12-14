using Moq;

namespace Coindesk.Test;

/// <summary>Mock Controllere工廠</summary>
/// <remarks>為以後可能擴充更多的類別預留設計空間</remarks>
public static class MockContextHelper
{
    /// <summary>新增上下文</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="context">上下文</param>
    /// <returns>上下文</returns>
    public static Mock<DatabaseContext> AddContext<T>(this Mock<DatabaseContext> context) where T : class
    {
        MockContextBase? mockController = null;
        if (typeof(T) == typeof(Currency))
            mockController = new MockCurrency();

        mockController?.Create(context);

        return context;
    }
}
