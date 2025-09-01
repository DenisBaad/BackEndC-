using FluentMigrator.Builders.Create.Table;
using FluentMigrator;

namespace Enderecos.Infrastructure.Migrations;
public abstract class BaseVersion : ForwardOnlyMigration
{
    protected ICreateTableColumnOptionOrWithColumnSyntax CreateTable(string table)
    {
        return Create.Table(table)
            .WithColumn("Id").AsGuid().PrimaryKey()
             .WithColumn("DataCadastro").AsDateTime().WithDefaultValue(SystemMethods.CurrentDateTime)
             .WithColumn("DataAtualizacao").AsDateTime();
    }
}