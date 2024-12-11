/// <summary>Coindesk幣別匯率資訊回傳Model</summary>
public class CurrencyInfoResponse
{
    /// <summary>幣別種類</summary>
    public string? Currency { get; set; }

    /// <summary>幣別名稱</summary>
    public string? Name { get; set; }

    /// <summary>匯率</summary>
    public decimal? Rate{ get; set; }
}