using Microsoft.Extensions.Configuration;

namespace Aquiles.Utils.Extensions;

public static class RepositorioExtension
{
    public static string GetNomeDatabase(this IConfiguration configuration)
    {
        return configuration.GetSection("ConnectionStrings:DatabaseName").Value ?? "";
    }

    public static string GetNomeConexao(this IConfiguration configuration)
    {
        return configuration.GetSection("ConnectionStrings:Connection").Value ?? "";
    }

    public static string GetConexaoCompleta(this IConfiguration configuration)
    {
        var databaseName = configuration.GetNomeDatabase();
        var conn = configuration.GetNomeConexao();

        return $"{conn}Database={databaseName}";
    }
}
