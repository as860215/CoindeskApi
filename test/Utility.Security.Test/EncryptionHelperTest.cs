using FluentAssertions;
using Xunit;

namespace Utility.Security.Test;
public class EncryptionHelperTest
{
    [Fact]
    public void Encrypt()
    {
        var result = EncryptionHelper.Encrypt("粉紅矮冬瓜");
        result.Should().Be("T6HwA5PxVWl8KmRqGwxUBQ==");
    }

    [Fact]
    public void Decrypt()
    {
        var result = EncryptionHelper.Decrypt("T6HwA5PxVWl8KmRqGwxUBQ==");
        result.Should().Be("粉紅矮冬瓜");
    }

    [Fact]
    public void EncryptAndDecrypt()
    {
        const string originalText = "as860215";
        var encrypt = EncryptionHelper.Encrypt(originalText);
        var decrypt = EncryptionHelper.Decrypt(encrypt);

        decrypt.Should().Be(originalText);
    }
}
