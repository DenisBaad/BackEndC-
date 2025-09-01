using Dapper;
using MySqlConnector;

namespace Aquiles.Utils.Services;
public static class Database
{
    public static void CriarDatabase(string connectionString, string nomeDataBase)
    {
        using var conn = new MySqlConnection(connectionString);
        var parametros = new DynamicParameters();
        parametros.Add("nome", nomeDataBase);
        var registros = conn.Query("SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @nome", parametros);
        if (!registros.Any())
        {
            conn.Query(CreateDatabaseStatement(nomeDataBase));
        }
    }

    private static string CreateDatabaseStatement(string nome)
    {
        return $"CREATE DATABASE {nome}";
    }
}
