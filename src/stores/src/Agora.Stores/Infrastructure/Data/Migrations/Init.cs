using FluentMigrator;

namespace Agora.Stores.Infrastructure.Data.Migrations;

[Migration(1)]
public sealed class Init : Migration
{
    private const string TableName = "store_request";

    public override void Up()
    {
        Create.Table(TableName)
            // ! Just to see a migration run.
            .WithColumn("id").AsGuid().PrimaryKey().WithDefault(SystemMethods.NewGuid)
            .WithColumn("name").AsString().NotNullable();
    }

    public override void Down()
    {
        Delete.Table(TableName);
    }
}