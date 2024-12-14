using Moq;

namespace Coindesk.Test;
public abstract class MockContextBase
{
    /// <summary>建立測試實例</summary>
    /// <param name="context">上下文</param>
    public abstract void Create(Mock<DatabaseContext> context);
}
