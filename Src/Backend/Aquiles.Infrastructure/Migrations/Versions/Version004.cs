using FluentMigrator;

namespace Aquiles.Infrastructure.Migrations.Versions;
[Migration(4, "Create table faturas")]

public class Version004 : BaseVersion
{
    public override void Up()
    {
        CreateTable("Faturas")
            .WithColumn("Status").AsInt16().NotNullable()
            .WithColumn("InicioVigencia").AsDateTime().NotNullable()
            .WithColumn("FimVigencia").AsDateTime().NotNullable()
            .WithColumn("DataPagamento").AsDateTime().Nullable()
            .WithColumn("DataVencimento").AsDateTime().NotNullable()
            .WithColumn("ValorTotal").AsDecimal(10, 2).NotNullable()
            .WithColumn("ValorDesconto").AsDecimal(10, 2).Nullable()
            .WithColumn("ValorPagamento").AsDecimal(10, 2).Nullable()
            .WithColumn("CodBoleto").AsString(45).Nullable()
            .WithColumn("IdTransacao").AsString(45).Nullable()
            .WithColumn("ClienteId").AsGuid().NotNullable().ForeignKey("FK_Faturas_Cliente_Id", "Clientes", "id").OnDelete(System.Data.Rule.Cascade)
            .WithColumn("UsuarioId").AsGuid().NotNullable().ForeignKey("FK_Faturas_Usuario_Id", "Usuarios", "id").OnDelete(System.Data.Rule.Cascade)
            .WithColumn("PlanoId").AsGuid().NotNullable().ForeignKey("FK_Faturas_Plano_Id", "Planos", "id").OnDelete(System.Data.Rule.Cascade);
    }
}
