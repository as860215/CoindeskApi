using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coindesk.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ILogger<CurrencyController> logger;
        private readonly BloggingContext context;

        public CurrencyController(ILogger<CurrencyController> logger, BloggingContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        /// <summary>取得匯率資料</summary>
        /// <param name="currencyType">幣別種類（null代表全部）</param>
        /// <returns>匯率資料</returns>
        [HttpGet]
        public List<Currency> Get(string? currencyType)
        {
            var currency = context.Currency.AsQueryable();

            if (!string.IsNullOrWhiteSpace(currencyType))
                currency = currency.Where(n => n.Type == currencyType);

            return currency.OrderBy(n => n.Type).ToList();
        }

        /// <summary>新增匯率資料</summary>
        /// <param name="currency">匯率資料</param>
        [HttpPost]
        public void Add(Currency currency)
        {
            ArgumentNullException.ThrowIfNull(currency);

            if (context.Currency.Any(n => n.Type == currency.Type))
                throw new InvalidOperationException($"指定的幣別 {currency.Type} 已存在");

            currency.LastUpdateDate = DateTimeOffset.Now;

            context.Currency.Add(currency);
            context.SaveChanges();
        }

        /// <summary>修改匯率資料</summary>
        /// <param name="currency">匯率資料</param>
        [HttpPatch]
        public void Modify(Currency currency)
        {
            ArgumentNullException.ThrowIfNull(currency);

            if (!context.Currency.Any(n => n.Type == currency.Type))
                throw new InvalidOperationException($"指定的幣別 {currency.Type} 不存在");

            currency.LastUpdateDate = DateTimeOffset.Now;

            context.Currency.Update(currency);
            context.SaveChanges();
        }

        /// <summary>刪除匯率資料</summary>
        /// <param name="currencyType">匯率種類</param>
        [HttpDelete]
        public void Delete(string currencyType)
        {
            if(string.IsNullOrWhiteSpace(currencyType))
                throw new ArgumentNullException(nameof(currencyType));

            var count = context.Currency.Where(n => n.Type == currencyType).ExecuteDelete();
            if (count == 0)
                throw new InvalidOperationException($"指定的幣別 {currencyType} 不存在");
            context.SaveChanges();
        }
    }
}
