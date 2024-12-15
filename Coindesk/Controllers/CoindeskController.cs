using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Coindesk.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CoindeskController : ControllerBase
    {
        private readonly ILogger<CoindeskController> logger;
        private readonly DatabaseContext context;
        private readonly IHttpClientFactory clientFactory;

        public CoindeskController(ILogger<CoindeskController> logger, DatabaseContext context, IHttpClientFactory clientFactory)
        {
            this.logger = logger;
            this.context = context;
            this.clientFactory = clientFactory;
        }

        /// <summary>���oCoindesk��T</summary>
        [HttpGet]
        public CoindeskResponse? GetCoindesk()
        {
            const string url = "https://api.coindesk.com/v1/bpi/currentprice.json";
            using var client = clientFactory.CreateClient();

            logger.LogInformation("[GetCoindesk][Http Get] start. url: {url}", url);
            var result = client.GetAsync(url).GetAwaiter().GetResult();
            var resultJson = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            logger.LogInformation("[GetCoindesk][Http Get] end. url: {url} | result: {result}", url, resultJson);
            return JsonSerializer.Deserialize<CoindeskResponse>(resultJson);
        }

        /// <summary>���oCoindesk��s�ɶ�</summary>
        /// <remarks>��s�ɶ��]yyyy/MM/dd HH:mm:ss�^</remarks>
        [HttpGet]
        public string? GetCoindeskUpdateTime() => GetCoindesk()?.Time?.UpdateTimeIso?.ToString("yyyy/MM/dd HH:mm:ss");

        /// <summary>���oCoindesk���O�ײv��T</summary>
        /// <returns>���O�ײv��T</returns>
        [HttpGet]
        public List<CurrencyInfoResponse>? GetCoindeskCurrencyInfo()
        {
            var coindeskInfo = GetCoindesk();
            if (coindeskInfo?.Bpi == null) return null;

            var currencies = context.Currency.Where(n => coindeskInfo.Bpi.Select(bpi => bpi.Key).Contains(n.Type)).ToDictionary(k => k.Type, v => v.Name);

            return coindeskInfo.Bpi.Select(n => new CurrencyInfoResponse
            {
                Currency = n.Key,
                Name = currencies.ContainsKey(n.Key) ? currencies[n.Key] : null,
                Rate = n.Value.Rate
            }).ToList();
        }
    }
}
