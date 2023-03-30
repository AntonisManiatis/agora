using FluentMigrator;

namespace Agora.Stores.Infrastructure.Data.Migrations;

[Migration(1)]
public sealed class Init : Migration
{
    private const string TableName = "store";

    public override void Up()
    {
        // TODO: Add a schema also.
        Create.Table(TableName)
            // ! Just to see a migration run.
            .WithColumn("id").AsGuid().PrimaryKey().WithDefault(SystemMethods.NewGuid)
            .WithColumn("user_id").AsGuid().NotNullable()
            .WithColumn("name").AsString(25).NotNullable()
            .WithColumn("status").AsString().NotNullable().WithDefaultValue("Pending")
            .WithColumn("tin").AsString(9).NotNullable();
    }

    public override void Down()
    {
        Delete.Table(TableName);
    }
}