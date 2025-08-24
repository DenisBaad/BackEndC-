using Aquiles.Application.Servicos;

namespace CommonTestUtilities.Cryptography;
public class PasswordEncryptBuilder
{
    public static PasswordEncrypt Build()
    {
        return new PasswordEncrypt("abc1234");
    }
}
