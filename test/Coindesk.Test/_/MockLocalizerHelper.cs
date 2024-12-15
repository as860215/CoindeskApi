using Microsoft.Extensions.Localization;
using Moq;

namespace Coindesk.Test;
public static class MockLocalizerHelper
{
    /// <summary>建立多語系Mock物件</summary>
    public static Mock<IStringLocalizer<T>> Create<T>() where T : class
    {
        var mockLocalizer = new Mock<IStringLocalizer<T>>();

        mockLocalizer.Setup(n => n["CurrencyNotFound"]).Returns(new LocalizedString("CurrencyNotFound", "Currency type {0} not found."));
        mockLocalizer.Setup(n => n["CurrencyAlreadyExists"]).Returns(new LocalizedString("CurrencyAlreadyExists", "Currency type {0} has already exists."));

        return mockLocalizer;
    }
}