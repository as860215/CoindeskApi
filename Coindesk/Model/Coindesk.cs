using System.Text.Json.Serialization;

public class CoindeskResponse
{
    [JsonPropertyName("time")]
    public TimeModel Time { get; set; }

    [JsonPropertyName("disclaimer")]
    public string? Disclaimer { get; set; }

    [JsonPropertyName("chartName")]
    public string? ChartName { get; set; }

    [JsonPropertyName("bpi")]
    public Dictionary<string, BpiDetail>? Bpi { get; set; }
}
public class TimeModel
{
    [JsonPropertyName("updated")]
    public string UpdateTime { get; set; }

    [JsonPropertyName("updatedISO")]
    public DateTimeOffset? UpdateTimeIso { get; set; }

    [JsonPropertyName("updateduk")]
    public string UpdateTimeDuk { get; set; }
}

public class BpiDetail
{
    [JsonPropertyName("code")]
    public string? Currency { get; set; }
    
    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }
    
    [JsonPropertyName("rate")]
    public string? RateText { get; set; }
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    [JsonPropertyName("rate_float")]
    public decimal? Rate { get; set; }

}