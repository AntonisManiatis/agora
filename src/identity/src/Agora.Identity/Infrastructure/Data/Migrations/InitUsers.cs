using FluentMigrator;

namespace Agora.Identity.Infrastructure.Data.Migrations;

[Migration(2)]
public sealed class InitUsers : Migration
{
    private const string SchemaName = "identity";
    private const string TableName = "user";

    public override void Up()
    {
        Create.Schema(SchemaName);

        Create.Table(TableName)
            .InSchema(SchemaName)
            .WithColumn("id").AsGuid().PrimaryKey().WithDefault(SystemMethods.NewGuid)
            .WithColumn("first_name").AsString(32).NotNullable()
            .WithColumn("last_name").AsString(32).NotNullable()
            .WithColumn("email").AsString(255).Unique()
            .WithColumn("password").AsString().NotNullable();
    }

    public override void Down()
    {
        Delete.Table(TableName).InSchema(SchemaName);
        Delete.Schema(SchemaName);
    }
}