using System.ComponentModel.DataAnnotations;
/// <summary>幣別</summary>
public class Currency
{
    /// <summary>幣別種類</summary>
    [Key]
    public string Type { get; set; }

    /// <summary>名稱</summary>
    public string? Name { get; set; }

    /// <summary>最後更新時間</summary>
    public DateTimeOffset? LastUpdateDate { get; set; }
}