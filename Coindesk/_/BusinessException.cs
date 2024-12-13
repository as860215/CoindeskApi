using Microsoft.EntityFrameworkCore;

/// <summary>業務邏輯錯誤訊息</summary>
/// <remarks>這會讓錯誤攔截接口將錯誤訊息往外部拋送</remarks>
public class BusinessException : Exception
{
    public BusinessException() : base() { }
    public BusinessException(string? message) : base(message) { }
}