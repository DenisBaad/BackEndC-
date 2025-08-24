using FluentMigrator;

namespace Aquiles.Infrastructure.Migrations.Versions;
[Migration(1, "Create table Usuarios")]
public class Version001 : BaseVersion
{
    public override void Up()
    {
        CreateTable("Usuarios")
            .WithColumn("Nome").AsString().NotNullable()
            .WithColumn("Email").AsString().NotNullable()
            .WithColumn("Senha").AsString().NotNullable();   
    }
}
