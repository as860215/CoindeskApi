using Microsoft.EntityFrameworkCore;

/// <summary>�~���޿���~�T��</summary>
/// <remarks>�o�|�����~�d�I���f�N���~�T�����~���߰e</remarks>
public class BusinessException : Exception
{
    public BusinessException() : base() { }
    public BusinessException(string? message) : base(message) { }
}