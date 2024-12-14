using Coindesk.Controllers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using Namotion.Reflection;
using System.Linq.Expressions;
using Xunit;

namespace Coindesk.Test;
public class MockCurrency : MockControllerBase
{
    public override void Create(Mock<DatabaseContext> context)
    {
        var currencies = new List<Currency>
        {
            new Currency("USD", "美金"), new Currency("EUR", "歐元"), new Currency("GBP", "英鎊"), new Currency("JPY", "日幣")
        };

        context.Setup(c => c.Currency).ReturnsDbSet(currencies);
        context.Setup(c => c.Currency.Add(It.IsAny<Currency>())).Callback<Currency>(currencies.Add);
        context.Setup(c => c.Currency.Update(It.IsAny<Currency>())).Callback<Currency>(currency =>
        {
            var element = currencies.FirstOrDefault(n => n.Type == currency.Type);
            if (element == null) return;
            else
            {
                element.Name = currency.Name;
                element.LastUpdateDate = DateTimeOffset.Now;
            }
        });
        context.Setup(c => c.Currency.Remove(It.IsAny<Currency>())).Callback<Currency>(currency => currencies.Remove(currency));
    }
}
