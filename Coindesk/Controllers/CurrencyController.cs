using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Coindesk.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ILogger<CurrencyController> logger;
        private readonly DatabaseContext context;
        private readonly IStringLocalizer localizer;
        private const string CurrencyNotFound = nameof(CurrencyNotFound);
        private const string CurrencyAlreadyExists = nameof(CurrencyAlreadyExists);

        public CurrencyController(ILogger<CurrencyController> logger, DatabaseContext context, IStringLocalizer<CurrencyController> localizer)
        {
            this.logger = logger;
            this.context = context;
            this.localizer = localizer;
        }

        /// <summary>取得匯率資料</summary>
        /// <param name="currencyType">幣別種類（null代表全部）</param>
        /// <returns>匯率資料</returns>
        [HttpGet]
        public List<Currency> Get(string? currencyType)
            => string.IsNullOrWhiteSpace(currencyType) ? context.Currency.OrderBy(n => n.Type).ToList()
                                                       : context.Currency.Where(n => n.Type == currencyType).ToList();

        /// <summary>新增匯率資料</summary>
        /// <param name="currency">匯率資料</param>
        [HttpPost]
        public void Add(Currency currency)
        {
            ArgumentNullException.ThrowIfNull(currency);

            if (context.Currency.Any(n => n.Type == currency.Type))
                throw new BusinessException(string.Format(localizer[CurrencyAlreadyExists], currency.Type));

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
                throw new BusinessException(string.Format(localizer[CurrencyNotFound], currency.Type));

            currency.LastUpdateDate = DateTimeOffset.Now;

            context.Currency.Update(currency);
            context.SaveChanges();
        }

        /// <summary>刪除匯率資料</summary>
        /// <param name="currencyType">匯率種類</param>
        [HttpDelete]
        public void Delete(string currencyType)
        {
            if (string.IsNullOrWhiteSpace(currencyType))
                throw new ArgumentNullException(nameof(currencyType));

            var element = context.Currency.SingleOrDefault(n => n.Type == currencyType) 
                          ?? throw new BusinessException(string.Format(localizer[CurrencyNotFound], currencyType));

            var count = context.Currency.Remove(element);
            context.SaveChanges();
        }
    }
}
