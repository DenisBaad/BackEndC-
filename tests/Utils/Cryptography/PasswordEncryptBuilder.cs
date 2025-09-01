using Aquiles.Utils.Services;

namespace CommonTestUtilities.Cryptography;
public class PasswordEncryptBuilder
{
    public static PasswordEncrypt Build()
    {
        return new PasswordEncrypt("abc1234");
    }
}
