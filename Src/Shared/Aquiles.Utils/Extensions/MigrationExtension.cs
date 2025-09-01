using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;

namespace Aquiles.Utils.Extensions;
public static class MigrationExtension
{
    public static void MigrateDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.ListMigrations();
        runner.MigrateUp();
    }
}
