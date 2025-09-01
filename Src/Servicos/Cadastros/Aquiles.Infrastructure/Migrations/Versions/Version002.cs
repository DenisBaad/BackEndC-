using FluentMigrator;

namespace Aquiles.Infrastructure.Migrations.Versions;
[Migration(2, "Create table Clientes")]
public class Version002 : BaseVersion
{
    public override void Up()
    {
        CreateTable("Clientes")
            .WithColumn("Codigo").AsInt32().NotNullable()
            .WithColumn("Tipo").AsInt16().NotNullable().WithDefaultValue(1)
            .WithColumn("CpfCnpj").AsString(14).Nullable()
            .WithColumn("Status").AsInt16().NotNullable().WithDefaultValue(1)
            .WithColumn("Nome").AsString(45).NotNullable()
            .WithColumn("Identidade").AsString(20).Nullable()
            .WithColumn("OrgaoExpedidor").AsString(10).Nullable()
            .WithColumn("DataNascimento").AsDateTime().Nullable()
            .WithColumn("NomeFantasia").AsString(45).Nullable()
            .WithColumn("Contato").AsString(40).NotNullable()
            .WithColumn("UsuarioId").AsGuid().NotNullable().ForeignKey("FK_Clientes_Usuario_Id", "Usuarios", "id").OnDelete(System.Data.Rule.Cascade);
    }
}
