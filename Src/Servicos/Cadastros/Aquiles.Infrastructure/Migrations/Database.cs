using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using Dapper;
using FluentMigrator.Runner;

namespace Aquiles.Infrastructure.Migrations;
public static class Database
{
    public static void Migrate(string connectionString, IServiceProvider serviceProvider)
    {
        CreateDatabase(connectionString);
        MigrationDatabase(serviceProvider);
    }
    private static void CreateDatabase(string connectionString)
    {
        var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString);
        var databaseName = connectionStringBuilder.Database;
        connectionStringBuilder.Remove("Database");
        using var dbConnection = new MySqlConnection(connectionStringBuilder.ConnectionString);
        var parameters = new DynamicParameters();
        parameters.Add("name", databaseName);
        var records = dbConnection.Query("SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @name", parameters);
        
        if (!records.Any())
            dbConnection.Execute($"CREATE DATABASE {databaseName}");
    }
    private static void MigrationDatabase(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        runner.ListMigrations();
        runner.MigrateUp();
    }
}
