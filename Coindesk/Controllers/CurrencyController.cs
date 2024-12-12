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

        /// <summary>���o�ײv���</summary>
        /// <param name="currencyType">���O�����]null�N������^</param>
        /// <returns>�ײv���</returns>
        [HttpGet]
        public List<Currency> Get(string? currencyType)
            => string.IsNullOrWhiteSpace(currencyType) ? context.Currency.OrderBy(n => n.Type).ToList()
                                                       : context.Currency.Where(n => n.Type == currencyType).ToList();

        /// <summary>�s�W�ײv���</summary>
        /// <param name="currency">�ײv���</param>
        [HttpPost]
        public void Add(Currency currency)
        {
            ArgumentNullException.ThrowIfNull(currency);

            if (context.Currency.Any(n => n.Type == currency.Type))
                throw new InvalidOperationException($"���w�����O {currency.Type} �w�s�b");

            currency.LastUpdateDate = DateTimeOffset.Now;

            context.Currency.Add(currency);
            context.SaveChanges();
        }

        /// <summary>�ק�ײv���</summary>
        /// <param name="currency">�ײv���</param>
        [HttpPatch]
        public void Modify(Currency currency)
        {
            ArgumentNullException.ThrowIfNull(currency);

            if (!context.Currency.Any(n => n.Type == currency.Type))
                throw new InvalidOperationException($"���w�����O {currency.Type} ���s�b");

            currency.LastUpdateDate = DateTimeOffset.Now;

            context.Currency.Update(currency);
            context.SaveChanges();
        }

        /// <summary>�R���ײv���</summary>
        /// <param name="currencyType">�ײv����</param>
        [HttpDelete]
        public void Delete(string currencyType)
        {
            if(string.IsNullOrWhiteSpace(currencyType))
                throw new ArgumentNullException(nameof(currencyType));

            var element = context.Currency.SingleOrDefault(n => n.Type == currencyType) 
                          ?? throw new InvalidOperationException($"���w�����O {currencyType} ���s�b");

            var count = context.Currency.Remove(element);
            context.SaveChanges();
        }
    }
}
