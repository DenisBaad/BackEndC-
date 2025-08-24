using FluentMigrator;

namespace Aquiles.Infrastructure.Migrations.Versions;
[Migration(3, "Create table planos")]

public class Version003 : BaseVersion
{
    public override void Up()
    {
        CreateTable("Planos")
            .WithColumn("Descricao").AsString().NotNullable()
            .WithColumn("ValorPlano").AsDecimal(10, 2).NotNullable()
            .WithColumn("QuantidadeUsuarios").AsInt16().NotNullable().WithDefaultValue(1)
            .WithColumn("VigenciaMeses").AsInt16().NotNullable()
            .WithColumn("UsuarioId").AsGuid().NotNullable().ForeignKey("FK_Planos_Usuario_Id", "Usuarios", "id").OnDelete(System.Data.Rule.Cascade);
    }
}
