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
            .WithColumn("user_id").AsGuid().NotNullable()
            .WithColumn("name").AsString().NotNullable()
            .WithColumn("status").AsString().NotNullable().WithDefaultValue("Pending")
            .WithColumn("tin").AsString(9).NotNullable();
    }

    public override void Down()
    {
        Delete.Table(TableName);
    }
}