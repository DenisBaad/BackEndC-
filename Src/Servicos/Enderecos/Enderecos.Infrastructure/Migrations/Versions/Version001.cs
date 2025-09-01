using FluentMigrator;

namespace Enderecos.Infrastructure.Migrations.Versions;
[Migration(1, "Create table Enderecos")]
public class Version001 : BaseVersion
{
    public override void Up()
    {
        CreateTable("Enderecos")
            .WithColumn("ClienteId").AsGuid().NotNullable()
            .WithColumn("Logradouro").AsString().NotNullable()
            .WithColumn("Numero").AsString().NotNullable()
            .WithColumn("Complemento").AsString().Nullable()
            .WithColumn("Bairro").AsString().NotNullable()
            .WithColumn("Cep").AsString().NotNullable()
            .WithColumn("Municipio").AsString().NotNullable()
            .WithColumn("UF").AsString(2).NotNullable()
            .WithColumn("Preferencial").AsBoolean().WithDefaultValue(false);
    }
}