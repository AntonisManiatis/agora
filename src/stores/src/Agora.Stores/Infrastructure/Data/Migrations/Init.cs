using FluentMigrator;

namespace Agora.Stores.Infrastructure.Data.Migrations;

[Migration(1)]
public sealed class Init : Migration
{
    private const string TableName = "store";

    public override void Up()
    {
        Create.Schema("stores");

        Create.Table(TableName)
            .InSchema("stores")
            .WithColumn("id").AsGuid().PrimaryKey().WithDefault(SystemMethods.NewGuid)
            .WithColumn("user_id").AsGuid().NotNullable()
            .WithColumn("name").AsString(25).NotNullable()
            .WithColumn("status").AsString().NotNullable().WithDefaultValue("Pending")
            .WithColumn("tin").AsString(9).NotNullable();

        Create.Table("store_category")
            .InSchema("stores")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("store_id").AsGuid()
            .WithColumn("name").AsString(255).Unique().NotNullable();

        Create.ForeignKey()
            .FromTable("store_category").InSchema("stores").ForeignColumn("store_id")
            .ToTable("store").InSchema("stores").PrimaryColumn("id");
    }

    public override void Down()
    {
        // ? Not sure if this cascades deletes.
        Delete.Schema("stores");

        Delete.Table(TableName);
        Delete.Table("store_category");
    }
}