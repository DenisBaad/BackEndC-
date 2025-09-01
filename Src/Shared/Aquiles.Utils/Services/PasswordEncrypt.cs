using System.Security.Cryptography;
using System.Text;

namespace Aquiles.Utils.Services;
public class PasswordEncrypt
{
    private readonly string _chaveAdicional;
    public PasswordEncrypt(string chaveAdicional)
    {
        _chaveAdicional = chaveAdicional;
    }

    public string Encript(string senha)
    {
        var senhaComChaveAdicional = $"{senha}{_chaveAdicional}";
        var bytes = Encoding.UTF8.GetBytes(senhaComChaveAdicional);
        byte[] hashBytes = SHA512.HashData(bytes);
        return StringBytes(hashBytes);
    }

    private static string StringBytes(byte[] bytes)
    {
        var sb = new StringBuilder();
        foreach (byte b in bytes)
        {
            var hex = b.ToString("x2");
            sb.Append(hex);
        }
        return sb.ToString();
    }
}
